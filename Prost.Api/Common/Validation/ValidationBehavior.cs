using ErrorOr;
using FluentValidation;
using Mediator;

namespace Prost.Api.Common.Validation;

internal sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(
                request,
                cancellationToken);

            errors.AddRange(result.ValidationErrors);
        }
        if (errors.Count is 0)
        {
            return await next(request, cancellationToken);
        }
        return (dynamic) errors;
    }
}
