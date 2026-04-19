using MediatR;
using System.Net;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Email;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.ChangeEmail;

public class ChangeEmailHandler(
    UserManager<User> userManager,
    IEmailService emailService,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    ILogger<ChangeEmailHandler> logger) : IRequestHandler<ChangeEmailCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return ApiResponse.ErrorResult(HttpStatusCode.Unauthorized, "Unauthorized.");

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return ApiResponse.ErrorResult(HttpStatusCode.Unauthorized, "Unauthorized.");

        var existingUser = await userManager.FindByEmailAsync(request.NewEmail);
        if (existingUser != null)
            return ApiResponse.ErrorResult(HttpStatusCode.Conflict, "Email is already taken.");

        var token = await userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var frontendUrl = configuration["Frontend:BaseUrl"];
        var confirmLink =
            $"{frontendUrl}/confirm-email-change?userId={user.Id}&token={encodedToken}&newEmail={Uri.EscapeDataString(request.NewEmail)}";

        await emailService.SendEmailAsync(EmailTemplates.ChangeEmail(user.Email!, confirmLink), cancellationToken);

        logger.LogInformation("Email change requested. UserId: {UserId}", user.Id);

        return ApiResponse.SuccessResult(HttpStatusCode.OK, "Email change request sent.");
    }
}