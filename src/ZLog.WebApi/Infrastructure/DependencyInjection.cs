using ZLog.WebApi.Infrastructure.Auth;
using ZLog.WebApi.Infrastructure.Data;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddAuth()
            .AddSqlLiteDb(configuration)
            .AddIdentity();
    
    private static IServiceCollection AddAuth( this IServiceCollection services)
    {
        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<TokenService>();
        services.AddScoped<RefreshTokenService>();

        return services;
    }
}