using System.ComponentModel.DataAnnotations;

namespace ZLog.WebApi.Infrastructure.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    
    [Required]
    public string Secret { get; init; } = string.Empty;
    
    [Required]
    public string Issuer { get; init; } = string.Empty;
    
    [Required]
    public string Audience { get; init; } = string.Empty;
    
    [Range(1, 1440)]
    public int AccessTokenExpirationMinutes { get; init; } = 5;
    
    [Range(1, 30)]
    public int RefreshTokenExpirationDays{ get; init; } = 7;
}