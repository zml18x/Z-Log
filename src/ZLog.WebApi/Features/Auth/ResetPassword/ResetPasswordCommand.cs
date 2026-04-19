using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.ResetPassword;

public record ResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmPassword) : IRequest<ApiResponse>;