using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Prost.Api.Common.Pagination;

internal sealed record CursorPage
{
    public string? Cursor { get; }
    public int Size { get; }
    public SortDirection Direction { get; }

    public CursorPage(
        string? cursor,
        int size = 10,
        SortDirection direction = SortDirection.Descending)
    {
        Cursor = cursor;
        Size = Math.Clamp(size, 1, 100);
        Direction = Enum.IsDefined(direction) switch
        {
            true => direction,
            false => SortDirection.Descending,
        };
    }

    public override string ToString() =>
        Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(
                        new { Cursor, Size, Direction }))));
}
