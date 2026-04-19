using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.ChangePassword;

public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest<ApiResponse>;