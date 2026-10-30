using Prost.Api.Domain.Primitives;
using System.Linq.Expressions;

namespace Prost.Api.Common.Pagination;

internal static class CursorExtensions
{
    private sealed class CapturedExpressions<TValue, TEntityId>(
        CursorKey<TValue, TEntityId> key)
        where TValue : notnull
        where TEntityId : struct, IEntityId
    {
        private sealed record Box<T>(T Value);

        private static MemberExpression Capture<T>(T value) =>
            Expression.Property(
                Expression.Constant(new Box<T>(value)),
                nameof(Box<>.Value));

        public Expression Id { get; } = Capture(key.Id);
        public Expression Value { get; } = Capture(key.Value);
    }

    private sealed class ParameterReplacer(
        ParameterExpression from,
        ParameterExpression to
    ) : ExpressionVisitor
    {
        protected override Expression VisitParameter(
            ParameterExpression node) =>
            node == from ? to : base.VisitParameter(node);
    }

    extension(SortDirection direction)
    {
        private ExpressionType Operator => direction switch
        {
            SortDirection.Ascending => ExpressionType.GreaterThan,
            SortDirection.Descending => ExpressionType.LessThan,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    private static Expression Rebind<TSource, TValue>(
        this Expression<Func<TSource, TValue>> selector,
        ParameterExpression item) =>
        new ParameterReplacer(
            selector.Parameters[0],
            item).Visit(selector.Body);

    private static BinaryExpression Compare(
        this SortDirection direction,
        Expression left,
        Expression right) =>
        Expression.MakeBinary(direction.Operator, left, right);

    public static Expression<Func<TSource, bool>> BuildCursorPredicate<TSource, TValue, TEntityId>(
        this CursorKey<TValue, TEntityId> key,
        Expression<Func<TSource, TValue>> selector,
        Expression<Func<TSource, TEntityId>> tiebreaker,
        SortDirection direction)
        where TValue : notnull
        where TEntityId : struct, IEntityId
    {
        var expressions = new CapturedExpressions<TValue, TEntityId>(key);

        var value = selector.Parameters[0];

        var predicate = Expression.OrElse(
            direction.Compare(selector.Body, expressions.Value),
            Expression.AndAlso(
                Expression.Equal(selector.Body, expressions.Value),
                direction.Compare(tiebreaker.Rebind(value), expressions.Id)));

        return Expression.Lambda<Func<TSource, bool>>(predicate, value);
    }
}
