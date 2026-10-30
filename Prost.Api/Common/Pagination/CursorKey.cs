using Prost.Api.Domain.Primitives;

namespace Prost.Api.Common.Pagination;

internal readonly record struct CursorKey<TValue, TEntityId>(TValue Value, TEntityId Id)
    where TValue : notnull
    where TEntityId : struct, IEntityId;
