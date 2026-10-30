using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Common.Pagination;
using Prost.Api.Data.Pagination;
using Prost.Api.Domain.Primitives;
using System.Linq.Expressions;

namespace Prost.Api.Data.Pagination;

internal static class CursorQueryableExtensions
{
    private static IOrderedQueryable<TSource> OrderBy<TSource, TValue>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TValue>> selector,
        SortDirection direction) =>
        direction switch
        {
            SortDirection.Ascending => source.OrderBy(selector),
            SortDirection.Descending => source.OrderByDescending(selector),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

    private static IOrderedQueryable<TSource> ThenBy<TSource, TValue>(
        this IOrderedQueryable<TSource> source,
        Expression<Func<TSource, TValue>> selector,
        SortDirection direction) =>
        direction switch
        {
            SortDirection.Ascending => source.ThenBy(selector),
            SortDirection.Descending => source.ThenByDescending(selector),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

    public static async Task<ErrorOr<CursorResponse>> ToCursorResponseAsync<TSource, TValue, TEntityId>(
        this IQueryable<TSource> source,
        CursorPage page,
        Func<TSource, dynamic> mapper,
        Expression<Func<TSource, TValue>> selector,
        Expression<Func<TSource, TEntityId>> tiebreaker,
        CancellationToken cancellationToken = default)
        where TEntityId : struct, IEntityId
        where TValue : notnull
    {
        if (!string.IsNullOrWhiteSpace(page.Cursor))
        {
            var key = CursorCodec.Decode<TValue, TEntityId>(page.Cursor);

            if (key.IsError) return key.Errors;

            source = source.Where(
                key.Value.BuildCursorPredicate(
                    selector, tiebreaker, page.Direction));
        }
        source = source
            .OrderBy(selector, page.Direction)
            .ThenBy(tiebreaker, page.Direction);

        var query = source.Take(page.Size + 1);

        var items = await query.ToListAsync(cancellationToken);

        var hasNext = items.Count > page.Size;

        if (hasNext) items.RemoveAt(items.Count - 1);

        string? nextCursor = null;

        if (hasNext && items.Count > 0)
        {
            var last = items[^1];

            nextCursor = CursorCodec.Encode(
                value: selector.Compile()(last),
                id: tiebreaker.Compile()(last));
        }
        return new CursorResponse(
            Items: [.. items.Select(mapper)],
            NextCursor: nextCursor,
            HasNext: hasNext);
    }
}
