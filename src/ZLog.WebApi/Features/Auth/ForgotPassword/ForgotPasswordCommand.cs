using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<ApiResponse>;