using MediatR;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.ResetPassword;

public class ResetPasswordHandler(UserManager<User> userManager, ILogger<ResetPasswordHandler> logger)
    : IRequestHandler<ResetPasswordCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
            
            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Invalid token or email.");
        }
        
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        
        var result = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new FieldError(e.Code, e.Description));
            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Password reset failed.", errors);
        }
        
        logger.LogInformation("Password reset successful. UserId: {UserId}", user.Id);
        
        return ApiResponse.SuccessResult(HttpStatusCode.OK, "Password reset successful.");
    }
}