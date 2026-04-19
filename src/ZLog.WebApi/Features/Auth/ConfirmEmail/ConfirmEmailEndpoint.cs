using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;

namespace ZLog.WebApi.Features.Auth.ConfirmEmail;

public class ConfirmEmailEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/confirm-email", async ([FromBody] ConfirmEmailCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Confirm Email")
            .AllowAnonymous()
            .Produces<ApiResponse>()
            .Produces<ApiResponse>(400);
    }
}