using Prost.Api.Domain.Primitives;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prost.Api.Common.Serialization;

internal sealed class EntityIdConverter<TEntityId, TValue> : JsonConverter<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId, TValue>
    where TValue : notnull
{
    public override TEntityId Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        TEntityId.From(
            JsonSerializer.Deserialize<TValue>(ref reader, options)
                ?? throw new JsonException(
                    $"JSON null cannot be converted to {typeof(TEntityId).Name}."));

    public override void Write(
        Utf8JsonWriter writer,
        TEntityId value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, options);
    }
}
