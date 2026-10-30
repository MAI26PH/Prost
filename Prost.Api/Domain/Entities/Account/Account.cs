using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Domain.Entities;

internal sealed class Account :
    Entity<AccountId>, ICreatedAt, IUpdatedAt
{
    public Username Username { get; private set; }

    public void UpdateUsername(Username username)
    {
        Username = username;
    }

    public Email Email { get; private set; }

    public void UpdateEmail(Email email)
    {
        Email = email;
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

    public PasswordCredential? PasswordCredential { get; }
    public ICollection<TokenFamily> TokenFamilies { get; } = [];
    public ICollection<Post> Posts { get; } = [];

    public uint RowVersion { get; }

    private Account(AccountId id) : base(id)
    {
        Username = default!;
        Email = default!;
    }

    private Account(
        AccountId id,
        Username username,
        Email email
    ) : base(id)
    {
        Username = username;
        Email = email;
    }

    public static Account Create(
        AccountId id,
        Username username,
        Email email) => new(
            id,
            username,
            email);
}
