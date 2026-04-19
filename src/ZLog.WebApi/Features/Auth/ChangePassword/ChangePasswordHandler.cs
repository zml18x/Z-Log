using MediatR;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.ChangePassword;

public class ChangePasswordHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<ChangePasswordHandler> logger) : IRequestHandler<ChangePasswordCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return ApiResponse.ErrorResult(HttpStatusCode.Unauthorized, "Unauthorized.");

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return ApiResponse.ErrorResult(HttpStatusCode.Unauthorized, "Unauthorized.");

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new FieldError(e.Code, e.Description));
            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Password change failed.", errors);
        }

        logger.LogInformation("Password changed. UserId: {UserId}", user.Id);

        return ApiResponse.SuccessResult(HttpStatusCode.OK, "Password changed successfully.");
    }
}