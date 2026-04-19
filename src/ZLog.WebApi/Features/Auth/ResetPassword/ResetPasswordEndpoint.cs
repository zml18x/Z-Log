using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;

namespace ZLog.WebApi.Features.Auth.ResetPassword;

public class ResetPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/reset-password", async ([FromBody] ResetPasswordCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Reset Password")
            .AllowAnonymous()
            .Produces<ApiResponse>()
            .Produces<ApiResponse>(400);
    }
}