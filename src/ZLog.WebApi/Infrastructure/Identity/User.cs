using Microsoft.AspNetCore.Identity;

namespace ZLog.WebApi.Infrastructure.Identity;

public class User : IdentityUser<Guid>
{
    public string DisplayName { get; init; } = null!;
    public string? ImageUrl { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}