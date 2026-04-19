using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.ConfirmEmailChange;

public record ConfirmEmailChangeCommand(string UserId, string Token, string NewEmail) : IRequest<ApiResponse>;