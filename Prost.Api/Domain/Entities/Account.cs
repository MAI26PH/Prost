using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Domain.Entities;

public sealed class Account :
    Entity<Account>,
    ICreatedAt,
    IUpdatedAt
{
    public Email Email { get; set; }

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

    public uint RowVersion { get; }

    private Account(Id<Account> id) : base(id)
    {
        Email = default!;
    }

    private Account(
        Id<Account> id,
        Email email
    ) : base(id)
    {
        Email = email;
    }

    public static Account Create(
        Id<Account> id,
        Email email) => new(
            id,
            email);
}
