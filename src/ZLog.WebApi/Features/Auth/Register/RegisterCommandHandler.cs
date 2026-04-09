using MediatR;
using System.Net;
using Microsoft.AspNetCore.Identity;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Features.Auth.Register;

public class RegisterCommandHandler(UserManager<User> userManager, ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, ApiResponse<RegisterResponse>>
{
    public async Task<ApiResponse<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Email = request.Email,
            UserName = request.Email,
            DisplayName = request.DisplayName
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => new FieldError(e.Code, e.Description));

            return ApiResponse<RegisterResponse>.ErrorResult(HttpStatusCode.BadRequest, "", errors);
        }

        logger.LogInformation("New user registered: {Email} with ID {UserId}", user.Email, user.Id);
        
        var response = new RegisterResponse(user.Id, user.Email, user.DisplayName);

        return ApiResponse<RegisterResponse>.SuccessResult(HttpStatusCode.Created, "Account created successfully.", response);
    }
}