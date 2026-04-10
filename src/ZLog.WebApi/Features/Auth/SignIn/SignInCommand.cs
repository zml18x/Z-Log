using MediatR;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Features.Auth.SignIn;

public record SignInCommand(string Email, string Password) : IRequest<ApiResponse<SignInResponse>>;