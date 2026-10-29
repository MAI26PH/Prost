using ErrorOr;
using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Domain.Entities;

internal sealed class RefreshToken :
    Entity<RefreshTokenId>, ICreatedAt
{
    public TokenFamilyId TokenFamilyId { get; }
    public TokenHash TokenHash { get; }
    public DateTimeOffset ExpiresAt { get; private set; }

    public bool IsExpired(DateTimeOffset now) =>
        ExpiresAt <= now;

    public DateTimeOffset? ConsumedAt { get; private set; }

    public bool IsConsumed() =>
        ConsumedAt.HasValue;

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

    private RefreshToken(RefreshTokenId id) : base(id)
    {
        TokenHash = default!;
    }

    private RefreshToken(
        RefreshTokenId id,
        TokenFamilyId tokenFamilyId,
        TokenHash tokenHash,
        DateTimeOffset expiresAt
    ) : base(id)
    {
        TokenFamilyId = tokenFamilyId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
    }

    public static ErrorOr<RefreshToken> Create(
        RefreshTokenId id,
        TokenFamilyId tokenFamilyId,
        TokenHash tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset now
    )
    {
        if (expiresAt <= now)
        {
            return RefreshTokenErrors.Expired;
        }
        return new RefreshToken(
            id,
            tokenFamilyId,
            tokenHash,
            expiresAt);
    }

    public ErrorOr<RefreshToken> Rotate(
        RefreshTokenId id,
        TokenFamily tokenFamily,
        TokenHash tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset now)
    {
        if (tokenFamily.Id != TokenFamilyId)
        {
            return RefreshTokenErrors.Unrotatable;
        }
        if (tokenFamily.IsRevoked())
        {
            return RefreshTokenErrors.Revoked;
        }
        if (IsConsumed())
        {
            return RefreshTokenErrors.Consumed;
        }
        if (IsExpired(now))
        {
            return RefreshTokenErrors.Expired;
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

        return refreshToken.Value;
    }
}
