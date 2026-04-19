using MediatR;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Email;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.ForgotPassword;

public class ForgotPasswordHandler(
    UserManager<User> userManager,
    IEmailService emailService,
    IConfiguration configuration,
    ILogger<ForgotPasswordHandler> logger) : IRequestHandler<ForgotPasswordCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
            return ApiResponse.SuccessResult(HttpStatusCode.OK, "If the email exists, a reset link has been sent.");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var frontendUrl = configuration["Frontend:BaseUrl"];
        var resetLink = $"{frontendUrl}/reset-password?token={encodedToken}&email={Uri.EscapeDataString(user.Email!)}";

        await emailService.SendEmailAsync(EmailTemplates.ResetPassword(user.Email!, resetLink), cancellationToken);

        logger.LogInformation("Password reset email sent. UserId: {UserId}", user.Id);

        return ApiResponse.SuccessResult(HttpStatusCode.OK, "If the email exists, a reset link has been sent.");
    }
}