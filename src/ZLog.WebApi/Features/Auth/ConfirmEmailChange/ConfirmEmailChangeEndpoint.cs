using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;

namespace ZLog.WebApi.Features.Auth.ConfirmEmailChange;

public class ConfirmEmailChangeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/confirm-email-change", async ([FromQuery] string userId, [FromQuery] string token,
                [FromQuery] string newEmail, ISender sender) =>
            {
                var response = await sender.Send(new ConfirmEmailChangeCommand(userId, token, newEmail));

                return response.ToApiResult();
            })
            .WithTags("Auth").WithName("Confirm New Email")
            .AllowAnonymous()
            .Produces<ApiResponse>()
            .Produces<ApiResponse>(400);
    }
}