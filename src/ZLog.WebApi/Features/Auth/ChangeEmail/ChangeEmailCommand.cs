using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.ChangeEmail;

public record ChangeEmailCommand(string NewEmail) : IRequest<ApiResponse>;