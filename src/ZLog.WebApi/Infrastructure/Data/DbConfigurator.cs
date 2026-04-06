using Microsoft.EntityFrameworkCore;

namespace ZLog.WebApi.Infrastructure.Data;

public static class DbConfigurator
{
    public static IServiceCollection AddSqlLiteDb(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<AppDbContext>(o =>
            o.UseSqlite(configuration.GetConnectionString("SqlLiteConnection")));
}