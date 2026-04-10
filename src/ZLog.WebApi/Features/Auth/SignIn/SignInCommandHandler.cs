using MediatR;
using System.Net;
using Microsoft.AspNetCore.Identity;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Auth;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.SignIn;

public class SignInCommandHandler(
    UserManager<User> userManager,
    TokenService tokenService,
    RefreshTokenService refreshTokenService,
    ILogger<SignInCommandHandler> logger)
    : IRequestHandler<SignInCommand, ApiResponse<SignInResponse>>
{
    public async Task<ApiResponse<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            logger.LogWarning("Failed sign-in attempt for email: {Email}", request.Email);

            return ApiResponse<SignInResponse>.ErrorResult(HttpStatusCode.Unauthorized, "Invalid credentials.");
        }


        if (userManager.Options.SignIn.RequireConfirmedEmail && !await userManager.IsEmailConfirmedAsync(user))
        {
            logger.LogWarning("Sign-in attempt with unconfirmed email. UserId: {UserId}", user.Id);

            return ApiResponse<SignInResponse>.ErrorResult(HttpStatusCode.Forbidden, "Email not confirmed.");
        }


        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = refreshTokenService.GenerateRefreshToken();

        await refreshTokenService.SaveAsync(user.Id, refreshToken, cancellationToken);

        logger.LogInformation("User signed in. UserId: {UserId}", user.Id);

        return ApiResponse<SignInResponse>
            .SuccessResult(HttpStatusCode.OK, "Successfully signed in.", new SignInResponse(accessToken, refreshToken));
    }
}