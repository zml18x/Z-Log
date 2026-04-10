namespace ZLog.WebApi.Infrastructure.Identity;

public class RefreshToken
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}