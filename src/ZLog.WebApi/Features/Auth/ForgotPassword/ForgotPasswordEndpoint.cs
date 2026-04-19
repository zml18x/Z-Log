using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;
using ZLog.WebApi.Features.Auth.Refresh;

namespace ZLog.WebApi.Features.Auth.ForgotPassword;

public class ForgotPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/forgot-password", async ([FromBody] ForgotPasswordCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Forgot Password")
            .AllowAnonymous()
            .Produces<ApiResponse<RefreshResponse>>();
    }
}