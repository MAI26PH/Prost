using ErrorOr;

namespace Prost.Api.Domain.Entities;

internal static class RefreshTokenErrors
{
    public static readonly Error Expired = Error.Unauthorized(
        code: "RefreshToken.Expired",
        description: "The refresh token has expired.");

    public static readonly Error Revoked = Error.Unauthorized(
        code: "RefreshToken.Revoked",
        description: "The refresh token has been revoked.");

    public static readonly Error Consumed = Error.Unauthorized(
        code: "RefreshToken.Consumed",
        description: "The refresh token has been consumed.");

    public static readonly Error Unrotatable = Error.Unauthorized(
        code: "RefreshToken.Unrotatable",
        description: "The refresh token could not be rotated.");
}
