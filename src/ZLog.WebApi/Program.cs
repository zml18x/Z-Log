using Serilog;
using Scalar.AspNetCore;
using ZLog.WebApi.Domain.Entities;
using ZLog.WebApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddOpenApi();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(o =>
    {
        o.Title = "Z-Log API";
    });
}

app.UseHttpsRedirection();

app.MapGroup("auth")
    .MapIdentityApi<User>();

await app.MigrateDatabase();

app.Run();