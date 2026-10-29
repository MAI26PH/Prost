using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Entities;

internal readonly record struct RefreshTokenId :
    IEntityId<RefreshTokenId, Guid>
{
    public Guid Value { get; }

    private RefreshTokenId(Guid value)
    {
        Value = value;
    }

    public static RefreshTokenId Create() =>
        new(Guid.CreateVersion7());

    public static RefreshTokenId From(Guid value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(RefreshTokenId other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(RefreshTokenId left, RefreshTokenId right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(RefreshTokenId left, RefreshTokenId right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(RefreshTokenId left, RefreshTokenId right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(RefreshTokenId left, RefreshTokenId right) =>
        left.CompareTo(right) <= 0;
}
