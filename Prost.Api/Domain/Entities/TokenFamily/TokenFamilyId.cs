using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Entities;

internal readonly record struct TokenFamilyId :
    IEntityId<TokenFamilyId, Guid>
{
    public Guid Value { get; }

    private TokenFamilyId(Guid value)
    {
        Value = value;
    }

    public static TokenFamilyId Create() =>
        new(Guid.CreateVersion7());

    public static TokenFamilyId From(Guid value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(TokenFamilyId other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(TokenFamilyId left, TokenFamilyId right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(TokenFamilyId left, TokenFamilyId right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(TokenFamilyId left, TokenFamilyId right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(TokenFamilyId left, TokenFamilyId right) =>
        left.CompareTo(right) <= 0;
}
