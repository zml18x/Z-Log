using System.Net;

namespace ZLog.WebApi.Shared.Responses;

public class ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<FieldError> Errors { get; init; } = [];
    public HttpStatusCode StatusCode { get; protected init; }

    public static ApiResponse SuccessResult(HttpStatusCode statusCode, string? message = null) => new()
    {
        Success = true,
        StatusCode = statusCode,
        Message = message
    };

    public static ApiResponse ErrorResult(HttpStatusCode statusCode, string? message = null, IEnumerable<FieldError>? errors = null) => new()
    {
        Success = false,
        StatusCode = statusCode,
        Message = message,
        Errors = errors?.ToList() ?? []
    };
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public static ApiResponse<T> SuccessResult(
        HttpStatusCode statusCode,
        string? message = null,
        T? data = default) => new()
    {
        Success = true,
        StatusCode = statusCode,
        Message = message,
        Data = data,
    };
    
    public new static ApiResponse<T> ErrorResult(
        HttpStatusCode statusCode,
        string? message = null,
        IEnumerable<FieldError>? errors = null) => new()
    {
        Success = false,
        StatusCode = statusCode,
        Message = message,
        Errors = errors?.ToList() ?? []
    };
}