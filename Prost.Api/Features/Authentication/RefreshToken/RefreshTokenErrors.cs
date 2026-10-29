using ErrorOr;

namespace Prost.Api.Features.Authentication;

internal static class RefreshTokenErrors
{
    public static readonly Error Invalid = Error.Unauthorized(
        code: "RefreshToken.Invalid",
        description: "The refresh token is invalid.");

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: "RefreshToken.ConcurrencyConflict",
        description: "The refresh failed due to a concurrency conflict.");
}
