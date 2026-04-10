using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Interfaces;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;

namespace ZLog.WebApi.Features.Auth.Refresh;

public class RefreshEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async ([FromBody] RefreshCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Refresh").AllowAnonymous()
            .Produces<ApiResponse<RefreshResponse>>()
            .Produces<ApiResponse<RefreshResponse>>(401);
    }
}