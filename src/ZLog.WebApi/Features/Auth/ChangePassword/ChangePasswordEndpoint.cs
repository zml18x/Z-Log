using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;
using ZLog.WebApi.Features.Auth.Refresh;

namespace ZLog.WebApi.Features.Auth.ChangePassword;

public class ChangePasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/change-password", async ([FromBody] ChangePasswordCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Change Password")
            .RequireAuthorization()
            .Produces<ApiResponse<RefreshResponse>>()
            .Produces<ApiResponse<RefreshResponse>>(400)
            .Produces<ApiResponse<RefreshResponse>>(401);
    }
}