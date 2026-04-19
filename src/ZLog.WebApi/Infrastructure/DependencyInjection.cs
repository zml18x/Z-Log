using Resend;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZLog.WebApi.Infrastructure.Auth;
using ZLog.WebApi.Infrastructure.Data;
using ZLog.WebApi.Infrastructure.Email;
using ZLog.WebApi.Infrastructure.Identity;

namespace ZLog.WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSqlLiteDb(configuration)
            .AddIdentity()
            .AddAuth(configuration)
            .AddResend(configuration);

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
            });

        services.AddAuthorization();

        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<TokenService>();
        services.AddScoped<RefreshTokenService>();

        return services;
    }

    private static IServiceCollection AddResend(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ResendClient>();

        services.Configure<ResendClientOptions>(o => { o.ApiToken = configuration["Resend:ApiKey"]!; });

        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IResend, ResendClient>();

        return services;
    }
}