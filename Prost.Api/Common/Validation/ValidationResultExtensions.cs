using FluentValidation.Results;
using Prost.Api.Domain.Primitives;

namespace Prost.Api.Common.Validation;

public static class ValidationResultExtensions
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
