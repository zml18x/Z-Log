namespace ZLog.WebApi.Infrastructure.Extensions;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSqlLiteDb(configuration)
            .AddIdentity();
}