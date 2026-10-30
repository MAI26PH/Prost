using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Entities;

internal readonly record struct PostId :
    IEntityId<PostId, Guid>
{
    public Guid Value { get; }

    private PostId(Guid value)
    {
        Value = value;
    }

    public static PostId Create() =>
        new(Guid.CreateVersion7());

    public static PostId From(Guid value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(PostId other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(PostId left, PostId right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(PostId left, PostId right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(PostId left, PostId right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(PostId left, PostId right) =>
        left.CompareTo(right) <= 0;
}
