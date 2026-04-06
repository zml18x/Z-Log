using Microsoft.EntityFrameworkCore;

namespace ZLog.WebApi.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (!Database.IsSqlite())
            modelBuilder.HasDefaultSchema("Z-Log");
    }
}