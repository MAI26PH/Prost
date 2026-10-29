namespace Prost.Api.Domain.Primitives;

internal abstract class Entity;

internal abstract class Entity<TEntityId>(
    TEntityId id
) : Entity, IEquatable<Entity<TEntityId>>
    where TEntityId : struct, IEntityId
{
    public TEntityId Id { get; } = id;

    public bool Equals(Entity<TEntityId>? other)
    {
        if (other is null) return false;

        if (other.GetType() != GetType()) return false;

        return EqualityComparer<TEntityId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj) =>
        obj is Entity<TEntityId> other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TEntityId>? left, Entity<TEntityId>? right)
    {
        if (left is null && right is null) return true;

        if (left is null || right is null) return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TEntityId>? left, Entity<TEntityId>? right) =>
        !(left == right);
}
