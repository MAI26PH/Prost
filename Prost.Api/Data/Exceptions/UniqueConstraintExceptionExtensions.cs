using EntityFramework.Exceptions.Common;
using Npgsql;

namespace Prost.Api.Data.Exceptions;

internal static class UniqueConstraintExceptionExtensions
{
    public static bool RefersTo<TProperty>(this UniqueConstraintException exception) =>
        exception.InnerException is PostgresException { ConstraintName: { } name } &&
        name.EndsWith(typeof(TProperty).Name, StringComparison.Ordinal);
}
