using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;

namespace Prost.Api.Domain.Entities;

public sealed class PasswordCredential :
    Entity<PasswordCredential>,
    ICreatedAt,
    IUpdatedAt
{
    public Id<Account> AccountId { get; }
    public string PasswordHash { get; set; }

    public DateTimeOffset CreatedAt { get; private set; }
    DateTimeOffset ICreatedAt.Value
    {
        get => CreatedAt;
        set => CreatedAt = value;
    }
    public DateTimeOffset? UpdatedAt { get; private set; }
    DateTimeOffset? IUpdatedAt.Value
    {
        get => UpdatedAt;
        set => UpdatedAt = value;
    }

    public Account Account { get; } = null!;

    public uint RowVersion { get; }

    private PasswordCredential(Id<PasswordCredential> id) : base(id)
    {
        PasswordHash = default!;
    }

    private PasswordCredential(
        Id<PasswordCredential> id,
        Id<Account> accountId,
        string passwordHash
    ) : base(id)
    {
        AccountId = accountId;
        PasswordHash = passwordHash;
    }

    public static PasswordCredential Create(
        Id<PasswordCredential> id,
        Id<Account> accountId,
        string passwordHash) => new(
            id,
            accountId,
            passwordHash);
}
