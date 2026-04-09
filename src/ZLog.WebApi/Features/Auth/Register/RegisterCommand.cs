using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.Register;

public record RegisterCommand(string Email, string DisplayName, string Password) : IRequest<ApiResponse<RegisterResponse>>;