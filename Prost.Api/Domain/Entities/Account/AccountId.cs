using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Entities;

internal readonly record struct AccountId :
    IEntityId<AccountId, Guid>
{
    public Guid Value { get; }

    private AccountId(Guid value)
    {
        Value = value;
    }

    public static AccountId Create() =>
        new(Guid.CreateVersion7());

    public static AccountId From(Guid value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(AccountId other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(AccountId left, AccountId right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(AccountId left, AccountId right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(AccountId left, AccountId right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(AccountId left, AccountId right) =>
        left.CompareTo(right) <= 0;
}
