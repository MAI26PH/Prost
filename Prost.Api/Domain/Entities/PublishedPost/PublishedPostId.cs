using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Entities;

internal readonly record struct PublishedPostId :
    IEntityId<PublishedPostId, Guid>
{
    public Guid Value { get; }

    private PublishedPostId(Guid value)
    {
        Value = value;
    }

    public static PublishedPostId Create() =>
        new(Guid.CreateVersion7());

    public static PublishedPostId From(Guid value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(PublishedPostId other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(PublishedPostId left, PublishedPostId right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(PublishedPostId left, PublishedPostId right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(PublishedPostId left, PublishedPostId right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(PublishedPostId left, PublishedPostId right) =>
        left.CompareTo(right) <= 0;
}
