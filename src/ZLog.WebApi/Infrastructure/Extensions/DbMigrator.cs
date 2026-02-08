using Microsoft.EntityFrameworkCore;
using ZLog.WebApi.Data.Context;
using ZLog.WebApi.Infrastructure.Utilities;

namespace ZLog.WebApi.Infrastructure.Extensions;

public static class DbMigrator
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        var config = services.GetRequiredService<IConfiguration>();
        
        var maxRetries = config.GetValue<int>("MigrationsSettings:MaxRetries");
        var delay = config.GetValue<int>("MigrationsSettings:Delay");
        
        logger.LogInformation("Starting database migration...");
        
        try 
        {
            await RetryHelper.ExecuteAsync(
                action: async () => await context.Database.MigrateAsync(),
                maxRetries: maxRetries,
                delay: TimeSpan.FromSeconds(delay),
                onRetry: (exception, attempt) => 
                    logger.LogWarning(
                        exception, 
                        "Migration attempt {Attempt} failed. Waiting {Delay}s...",
                        attempt,
                        delay)
            );

            logger.LogInformation("Database migrated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Fatal error: Migration failed after {RetryCount} attempts.", maxRetries);
            throw;
        }
    }
}