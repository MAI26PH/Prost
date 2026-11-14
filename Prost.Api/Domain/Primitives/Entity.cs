namespace Prost.Api.Domain.Primitives;

public interface IEntity;

public abstract class Entity<TSelf>(Id<TSelf> id) :
    IEntity,
    IEquatable<Entity<TSelf>>
    where TSelf : Entity<TSelf>
{
    public Id<TSelf> Id { get; } = id;

    public bool Equals(Entity<TSelf>? other)
    {
        if (other is null) return false;

        if (other.GetType() != GetType()) return false;

        return Id == other.Id;
    }

    public override bool Equals(object? obj) =>
        obj is Entity<TSelf> other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TSelf>? left, Entity<TSelf>? right)
    {
        if (left is null && right is null) return true;

        if (left is null || right is null) return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TSelf>? left, Entity<TSelf>? right) =>
        !(left == right);
}
