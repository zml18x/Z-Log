using Serilog;
using Scalar.AspNetCore;
using ZLog.WebApi.Infrastructure;
using ZLog.WebApi.Infrastructure.Data;

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

await app.MigrateDatabase();

app.Run();