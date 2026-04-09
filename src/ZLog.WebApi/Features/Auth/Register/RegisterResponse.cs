namespace ZLog.WebApi.Features.Auth.Register;

public record RegisterResponse(Guid UserId, string Email, string DisplayName);