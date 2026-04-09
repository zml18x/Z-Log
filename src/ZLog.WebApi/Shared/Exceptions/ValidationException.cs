using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Shared.Exceptions;

public class ValidationException(IEnumerable<FieldError> errors) : Exception("Validation Error")
{
    public IReadOnlyList<FieldError> Errors { get; } = errors.ToList();
}