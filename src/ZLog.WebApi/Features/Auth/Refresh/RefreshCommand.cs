using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.Refresh;

public record RefreshCommand(string AccessToken, string RefreshToken) : IRequest<ApiResponse<RefreshResponse>>;