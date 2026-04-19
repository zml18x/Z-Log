using MediatR;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.ConfirmEmailChange;

public class ConfirmEmailChangeHandler(UserManager<User> userManager, ILogger<ConfirmEmailChangeHandler> logger)
    : IRequestHandler<ConfirmEmailChangeCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Invalid request.");

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var result = await userManager.ChangeEmailAsync(user, request.NewEmail, decodedToken);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new FieldError(e.Code, e.Description));
            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Email change failed.", errors);
        }

        await userManager.SetUserNameAsync(user, request.NewEmail);

        logger.LogInformation("Email changed. UserId: {UserId}", user.Id);

        return ApiResponse.SuccessResult(HttpStatusCode.OK, "Email changed successfully.");
    }
}