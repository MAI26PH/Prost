using ErrorOr;
using Prost.Api.Common.Serialization;
using Prost.Api.Domain.Primitives;
using System.Buffers.Text;
using System.Text.Json;

namespace Prost.Api.Common.Pagination;

internal static class CursorCodec
{
    private static readonly JsonSerializerOptions options = new()
    {
        Converters = { new JsonEntityIdConverter() }
    };

    public static string Encode<TValue, TEntityId>(TValue value, TEntityId id)
        where TValue : notnull
        where TEntityId : struct, IEntityId =>
        Base64Url.EncodeToString(
            JsonSerializer.SerializeToUtf8Bytes(
                new CursorKey<TValue, TEntityId>(value, id),
                options));

    public static ErrorOr<CursorKey<TValue, TEntityId>> Decode<TValue, TEntityId>(string cursor)
        where TValue : notnull
        where TEntityId : struct, IEntityId
    {
        try
        {
            var result = JsonSerializer.Deserialize<CursorKey<TValue, TEntityId>?>(
                Base64Url.DecodeFromChars(cursor),
                options);

            if (result is not { } key) return CursorErrors.Empty;

            return key;
        }
        catch (Exception ex) when (ex is FormatException or JsonException or NotSupportedException)
        {
            return CursorErrors.Undecodable;
        }
    }
}
