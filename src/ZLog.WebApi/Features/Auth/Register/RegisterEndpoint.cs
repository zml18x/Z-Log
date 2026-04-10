using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Interfaces;

namespace ZLog.WebApi.Features.Auth.Register;

public class RegisterEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async ([FromBody] RegisterCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.ToApiResult();
            }).WithTags("Auth").WithName("Register") .AllowAnonymous()
            .Produces<ApiResponse<RegisterResponse>>(StatusCodes.Status201Created)
            .Produces<ApiResponse<RegisterResponse>>(StatusCodes.Status400BadRequest);
    }
}