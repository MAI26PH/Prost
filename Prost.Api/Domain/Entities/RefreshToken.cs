using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;

namespace Prost.Api.Domain.Entities;

public sealed class RefreshToken :
    Entity<RefreshToken>,
    ICreatedAt
{
    public static class Errors
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

    public Id<TokenFamily> TokenFamilyId { get; }
    public string TokenHash { get; }
    public DateTimeOffset ExpiresAt { get; private set; }

    public bool IsExpired(DateTimeOffset now) => ExpiresAt <= now;

    public DateTimeOffset? ConsumedAt { get; private set; }

    public bool IsConsumed() => ConsumedAt.HasValue;

    public void Consume(DateTimeOffset now)
    {
        ConsumedAt ??= now;
    }

    public DateTimeOffset CreatedAt { get; private set; }
    DateTimeOffset ICreatedAt.Value
    {
        get => CreatedAt;
        set => CreatedAt = value;
    }

    public TokenFamily TokenFamily { get; } = null!;

    public uint RowVersion { get; }

    private RefreshToken(Id<RefreshToken> id) : base(id)
    {
        TokenHash = default!;
    }

    private RefreshToken(
        Id<RefreshToken> id,
        Id<TokenFamily> tokenFamilyId,
        string tokenHash,
        DateTimeOffset expiresAt
    ) : base(id)
    {
        TokenFamilyId = tokenFamilyId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
    }

    public static ErrorOr<RefreshToken> Create(
        Id<RefreshToken> id,
        Id<TokenFamily> tokenFamilyId,
        string tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset now)
    {
        if (expiresAt <= now)
        {
            return Errors.Expired;
        }
        return new RefreshToken(
            id,
            tokenFamilyId,
            tokenHash,
            expiresAt);
    }

    public ErrorOr<RefreshToken> Rotate(
        Id<RefreshToken> id,
        TokenFamily tokenFamily,
        string tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset now)
    {
        if (tokenFamily.Id != TokenFamilyId)
        {
            return Errors.Unrotatable;
        }
        if (tokenFamily.IsRevoked())
        {
            return Errors.Revoked;
        }
        if (IsConsumed())
        {
            return Errors.Consumed;
        }
        if (IsExpired(now))
        {
            return Errors.Expired;
        }
        var refreshToken = Create(
            id,
            tokenFamily.Id,
            tokenHash,
            expiresAt,
            now);

        if (refreshToken.IsError)
        {
            return refreshToken.Errors;
        }
        Consume(now);

        return refreshToken;
    }
}
