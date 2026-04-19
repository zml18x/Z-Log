using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Shared.Extensions;

public static class ApiResults
{
    public static IResult ToApiResult<T>(this ApiResponse<T> response) =>
        Results.Json(response, statusCode: (int)response.StatusCode);
    
    public static IResult ToApiResult(this ApiResponse response) =>
        Results.Json(response, statusCode: (int)response.StatusCode);
}