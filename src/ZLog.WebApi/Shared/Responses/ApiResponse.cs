namespace ZLog.WebApi.Shared.Responses;

public class ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<FieldError> Errors { get; init; } = [];
    public int StatusCode { get; init; }

    public static ApiResponse SuccessResult(int statusCode, string? message = null) => new()
    {
        Success = true,
        StatusCode = statusCode,
        Message = message
    };

    public static ApiResponse ErrorResult(int statusCode, string? message = null, IEnumerable<FieldError>? errors = null) => new()
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
        int statusCode,
        string? message = null,
        T? data = default) => new()
    {
        Success = true,
        StatusCode = statusCode,
        Message = message,
        Data = data,
    };
}