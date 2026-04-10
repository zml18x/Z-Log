using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;

namespace ZLog.WebApi.Features.Auth.SignIn;

public class SignInEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/sign-in", async ([FromBody] SignInCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("SignIn").AllowAnonymous()
            .Produces<ApiResponse<SignInResponse>>()
            .Produces<ApiResponse<SignInResponse>>(401)
            .Produces<ApiResponse<SignInResponse>>(403);
    }
}