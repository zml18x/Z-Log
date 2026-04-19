using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ZLog.WebApi.Shared.Exceptions;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Shared.Services;

public class UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var id = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return Guid.TryParse(id, out var guid) ? guid : null;
        }
    }

    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);

    public string? DisplayName => Principal?.FindFirstValue(ClaimTypes.Name);

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
    
    public async Task<User> GetRequiredUserAsync(CancellationToken cancellationToken = default)
    {
        if (UserId == null)
            throw new UnauthorizedException("Unauthorized.");

        var user = await userManager.FindByIdAsync(UserId.Value.ToString());

        return user ?? throw new UnauthorizedException("Unauthorized.");
    }
    
    public async Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (UserId == null) 
            return null;
        
        return await userManager.FindByIdAsync(UserId.Value.ToString());
    }
}