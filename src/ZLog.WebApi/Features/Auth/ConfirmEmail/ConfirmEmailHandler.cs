using MediatR;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.ConfirmEmail;

public class ConfirmEmailHandler(UserManager<User> userManager, ILogger<ConfirmEmailHandler> logger)
    : IRequestHandler<ConfirmEmailCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Invalid token or userId.");

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new FieldError(e.Code, e.Description));

            return ApiResponse.ErrorResult(HttpStatusCode.BadRequest, "Email confirmation failed.", errors);
        }

        logger.LogInformation("Email confirmed. UserId: {UserId}", user.Id);

        return ApiResponse.SuccessResult(HttpStatusCode.OK, "Email confirmed successfully.");
    }
}