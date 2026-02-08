using Microsoft.EntityFrameworkCore;
using ZLog.WebApi.Data.Context;

namespace ZLog.WebApi.Infrastructure.Extensions;

public static class DbConfigurator
{
    public static IServiceCollection AddSqlLiteDb(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<AppDbContext>(o =>
            o.UseSqlite(configuration.GetConnectionString("SqlLiteConnection")));
}