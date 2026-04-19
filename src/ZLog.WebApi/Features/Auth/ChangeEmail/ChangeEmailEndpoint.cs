using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;

namespace ZLog.WebApi.Features.Auth.ChangeEmail;

public class ChangeEmailEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/change-email", async ([FromBody] ChangeEmailCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Change Email")
            .RequireAuthorization()
            .Produces<ApiResponse>()
            .Produces<ApiResponse>(400)
            .Produces<ApiResponse>(401)
            .Produces<ApiResponse>(409);
    }
}