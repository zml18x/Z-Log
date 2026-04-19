using MediatR;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Email;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.Register;

public class RegisterCommandHandler(
    UserManager<User> userManager,
    IEmailService emailService,
    IConfiguration configuration,
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, ApiResponse<RegisterResponse>>
{
    public async Task<ApiResponse<RegisterResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            DisplayName = request.DisplayName
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => new FieldError(e.Code, e.Description));

            return ApiResponse<RegisterResponse>.ErrorResult(HttpStatusCode.BadRequest, "", errors);
        }

        logger.LogInformation("New user registered with ID {UserId}", user.Id);

        if (userManager.Options.SignIn.RequireConfirmedEmail)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var frontendUrl = configuration["Frontend:BaseUrl"];
            var confirmLink = $"{frontendUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

            await emailService.SendEmailAsync(EmailTemplates.ConfirmEmail(user.Email, confirmLink), cancellationToken);
        }

        var response = new RegisterResponse(user.Id, user.Email, user.DisplayName);

        return ApiResponse<RegisterResponse>.SuccessResult(HttpStatusCode.Created, "Account created successfully.",
            response);
    }
}