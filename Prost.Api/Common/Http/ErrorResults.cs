using ErrorOr;

namespace Prost.Api.Common.Http;

internal static class ErrorResults
{
    public static IResult Problem(IReadOnlyList<Error> errors)
    {
        if (errors.Count is 0) return Results.Problem();

        if (errors.All(e => e.Type is ErrorType.Validation))
        {
            return Results.ValidationProblem(errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(
                        e => e.Description).ToArray()));
        }
        return Problem(errors[0]);
    }

    public static IResult Problem(Error error) =>
        Results.Problem(
            title: error.Code,
            statusCode: error.Type.StatusCode,
            detail: error.Description);
}
