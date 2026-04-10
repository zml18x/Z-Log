namespace ZLog.WebApi.Features.Auth.SignIn;

public record SignInResponse(string AccessToken, string RefreshToken);