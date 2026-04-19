using System.Net;
using ZLog.WebApi.Shared.Responses;
using ZLog.WebApi.Shared.Exceptions;

namespace ZLog.WebApi.Shared.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message, errors) = ex switch
        {
            ValidationException ve  => (HttpStatusCode.BadRequest, ve.Message, ve.Errors),
            UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message, null),
            ArgumentNullException or ArgumentException  => (HttpStatusCode.BadRequest, ex.Message, null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };

        var response = ApiResponse.ErrorResult(statusCode, message, errors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}