using ErrorOr;

namespace Prost.Api.Features.Accounts;

internal static class UpdateEmailErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: $"UpdateEmail.NotFound",
        description: "The account was not found.");

    public static readonly Error Taken = Error.Conflict(
        code: "UpdateEmail.Taken",
        description: "The email is already taken.");

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: $"UpdateEmail.ConcurrencyConflict",
        description: "The update failed due to a concurrency conflict.");
}
