using Microsoft.AspNetCore.Identity;
using ZLog.WebApi.Data.Context;
using ZLog.WebApi.Domain.Entities;

namespace ZLog.WebApi.Infrastructure.Extensions;

public static class IdentityConfigurator
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>(o =>
            {
                o.SignIn.RequireConfirmedEmail = false;
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 8;
                o.Password.RequireDigit = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}