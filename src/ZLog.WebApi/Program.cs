using MediatR;
using Serilog;
using FluentValidation;
using Scalar.AspNetCore;
using ZLog.WebApi.Infrastructure;
using ZLog.WebApi.Infrastructure.Data;
using ZLog.WebApi.Shared.Behaviours;
using ZLog.WebApi.Shared.Extensions;
using ZLog.WebApi.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddEndpoints()
    .AddInfrastructure(builder.Configuration)
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    });

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

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.MapEndpoints();

await app.MigrateDatabase();

app.Run();