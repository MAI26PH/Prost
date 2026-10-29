using ErrorOr;
using FluentValidation.Results;

namespace Prost.Api.Common.Validation;

internal static class ValidationResultExtensions
{
    extension(ValidationResult result)
    {
        public IReadOnlyList<Error> ValidationErrors =>
            result.Errors.ConvertAll(
                failure => Error.Validation(
                    code: failure.PropertyName,
                    description: failure.ErrorMessage));
    }
}
