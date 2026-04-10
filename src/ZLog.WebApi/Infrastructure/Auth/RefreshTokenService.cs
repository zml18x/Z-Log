using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ZLog.WebApi.Infrastructure.Data;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Infrastructure.Auth;

public class RefreshTokenService(AppDbContext context, IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwt = jwtOptions.Value;
    
    public string GenerateRefreshToken() =>
        WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(64));
    
    public async Task SaveAsync(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays)
        };

        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetValidAsync(string token, Guid userId,
        CancellationToken cancellationToken = default)
        => await context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Token == token && x.UserId == userId && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow,
                cancellationToken);

    public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (refreshToken is null)
            return;

        refreshToken.IsRevoked = true;

        await context.SaveChangesAsync(cancellationToken);
    }
}