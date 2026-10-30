using ErrorOr;

namespace Prost.Api.Common.Pagination;

internal static class CursorErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Cursor.Empty",
        description: "The cursor is empty.");

    public static readonly Error Undecodable = Error.Validation(
        code: "Cursor.Undecodable",
        description: "The cursor could not be decoded.");
}
