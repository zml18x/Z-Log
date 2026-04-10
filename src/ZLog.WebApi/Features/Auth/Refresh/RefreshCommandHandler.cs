using MediatR;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Auth;

namespace ZLog.WebApi.Features.Auth.Refresh;

public class RefreshCommandHandler(
    RefreshTokenService refreshTokenService,
    TokenService tokenService,
    ILogger<RefreshCommandHandler> logger) : IRequestHandler<RefreshCommand, ApiResponse<RefreshResponse>>
{
    public async Task<ApiResponse<RefreshResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        ClaimsPrincipal claimsPrincipal;

        try
        {
            claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        }
        catch (SecurityTokenException)
        {
            return ApiResponse<RefreshResponse>.ErrorResult(HttpStatusCode.Unauthorized, "Invalid access token.");
        }

        var userId = tokenService.GetUserIdFromPrincipal(claimsPrincipal);
        if (userId == null || userId == Guid.Empty)
            return ApiResponse<RefreshResponse>.ErrorResult(HttpStatusCode.Unauthorized, "Invalid access token.");

        var existingRefreshToken = await refreshTokenService.GetValidAsync(request.RefreshToken, userId.Value, cancellationToken);

        if (existingRefreshToken == null)
            return ApiResponse<RefreshResponse>
                .ErrorResult(HttpStatusCode.Unauthorized, "Invalid or expired refresh token.");

        await refreshTokenService.RevokeAsync(existingRefreshToken.Token, cancellationToken);

        var newAccessToken = tokenService.GenerateAccessToken(existingRefreshToken.User);
        var newRefreshToken = refreshTokenService.GenerateRefreshToken();

        await refreshTokenService.SaveAsync(existingRefreshToken.User.Id, newRefreshToken, cancellationToken);

        logger.LogInformation("Refresh token rotated. UserId: {UserId}", existingRefreshToken.User.Id);

        return ApiResponse<RefreshResponse>.SuccessResult(HttpStatusCode.OK, "Token refreshed successfully.",
            new RefreshResponse(newAccessToken, newRefreshToken));
    }
}