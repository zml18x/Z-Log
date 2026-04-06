using ZLog.WebApi.Infrastructure.Data;

namespace ZLog.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSqlLiteDb(configuration);
}