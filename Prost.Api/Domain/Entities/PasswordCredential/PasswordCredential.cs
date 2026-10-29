using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Domain.Entities;

internal sealed class PasswordCredential :
    Entity<PasswordCredentialId>, ICreatedAt, IUpdatedAt
{
    public AccountId AccountId { get; }
    public PasswordHash PasswordHash { get; private set; }

    public void UpdatePasswordHash(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }

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

    private PasswordCredential(PasswordCredentialId id) : base(id)
    {
        PasswordHash = default!;
    }

    private PasswordCredential(
        PasswordCredentialId id,
        AccountId accountId,
        PasswordHash passwordHash
    ) : base(id)
    {
        AccountId = accountId;
        PasswordHash = passwordHash;
    }

    public static PasswordCredential Create(
        PasswordCredentialId id,
        AccountId accountId,
        PasswordHash passwordHash) => new(
            id,
            accountId,
            passwordHash);
}
