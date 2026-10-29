using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;

namespace Prost.Api.Domain.Entities;

internal sealed class TokenFamily :
    Entity<TokenFamilyId>, ICreatedAt
{
    public AccountId AccountId { get; }
    public DateTimeOffset? RevokedAt { get; private set; }

    public bool IsRevoked() =>
        RevokedAt.HasValue;

    public void Revoke(DateTimeOffset now)
    {
        RevokedAt ??= now;
    }

    public DateTimeOffset CreatedAt { get; private set; }
    DateTimeOffset ICreatedAt.Value
    {
        get => CreatedAt;
        set => CreatedAt = value;
    }

    public Account Account { get; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; } = [];

    private TokenFamily(
        TokenFamilyId id,
        AccountId accountId
    ) : base(id)
    {
        AccountId = accountId;
    }

    public static TokenFamily Create(
        TokenFamilyId id,
        AccountId accountId) => new(
            id,
            accountId);
}
