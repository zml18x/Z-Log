using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.ConfirmEmail;

public record ConfirmEmailCommand(Guid UserId, string Token) : IRequest<ApiResponse>;