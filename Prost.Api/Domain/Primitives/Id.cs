using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Prost.Api.Domain.Primitives;

public interface IId;

[SuppressMessage(
    "Naming",
    "CA1000: Do not declare static members on generic types")]
public readonly record struct Id<TEntity> :
    IId,
    IComparable<Id<TEntity>>,
    IComparisonOperators<Id<TEntity>, Id<TEntity>, bool>
    where TEntity : Entity<TEntity>
{
    public Guid Value { get; }

    private Id(Guid value)
    {
        Value = value;
    }

    public static Id<TEntity> Create() =>
        new(Guid.CreateVersion7());

    public static Id<TEntity> From(Guid value) =>
        new(value);

    public static implicit operator Guid(Id<TEntity> id) =>
        id.Value;

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(Id<TEntity> other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(Id<TEntity> left, Id<TEntity> right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(Id<TEntity> left, Id<TEntity> right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(Id<TEntity> left, Id<TEntity> right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(Id<TEntity> left, Id<TEntity> right) =>
        left.CompareTo(right) <= 0;
}
