using ErrorOr;

namespace Prost.Api.Features.Accounts;

internal static class UpdatePasswordErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "UpdatePassword.NotFound",
        description: "The account was not found.");

    public static readonly Error NotSet = Error.NotFound(
        code: "UpdatePassword.NotSet",
        description: "The password is not set.");

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: "UpdatePassword.ConcurrencyConflict",
        description: "The update failed due to a concurrency conflict.");
}
