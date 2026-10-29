using System.Numerics;

namespace Prost.Api.Domain.Primitives;

internal interface IEntityId;

internal interface IEntityId<TEntityId, TValue> :
    IEntityId,
    IComparable<TEntityId>,
    IComparisonOperators<TEntityId, TEntityId, bool>
    where TEntityId : struct, IEntityId<TEntityId, TValue>
    where TValue : notnull
{
    TValue Value { get; }

    static abstract TEntityId Create();

    static abstract TEntityId From(TValue value);
}
