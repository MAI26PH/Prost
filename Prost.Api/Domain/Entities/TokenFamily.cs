using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;

namespace Prost.Api.Domain.Entities;

public sealed class TokenFamily :
    Entity<TokenFamily>,
    ICreatedAt
{
    public Id<Account> AccountId { get; }
    public DateTimeOffset? RevokedAt { get; private set; }

    public bool IsRevoked() => RevokedAt.HasValue;

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

    private TokenFamily(
        Id<TokenFamily> id,
        Id<Account> accountId
    ) : base(id)
    {
        AccountId = accountId;
    }

    public static TokenFamily Create(
        Id<TokenFamily> id,
        Id<Account> accountId) => new(
            id,
            accountId);
}
