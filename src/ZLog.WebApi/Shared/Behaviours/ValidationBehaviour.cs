using MediatR;
using FluentValidation;
using ZLog.WebApi.Shared.Responses;

namespace ZLog.WebApi.Shared.Behaviours;

public sealed class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var errors = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .Select(f => new FieldError(f.PropertyName, f.ErrorMessage))
            .ToList();

        if (errors.Count != 0)
            throw new ZLog.WebApi.Shared.Exceptions.ValidationException(errors);

        return await next(cancellationToken);
    }
}