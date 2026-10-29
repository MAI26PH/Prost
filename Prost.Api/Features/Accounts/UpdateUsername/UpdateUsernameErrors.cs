using ErrorOr;

namespace Prost.Api.Features.Accounts;

internal static class UpdateUsernameErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "UpdateUsername.NotFound",
        description: "The account was not found.");

    public static readonly Error Taken = Error.Conflict(
        code: "UpdateUsername.Taken",
        description: "The username is already taken.");

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: "UpdateUsername.ConcurrencyConflict",
        description: "The update failed due to a concurrency conflict.");
}
