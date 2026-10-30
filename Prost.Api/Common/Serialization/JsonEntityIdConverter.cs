using Prost.Api.Domain.Primitives;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prost.Api.Common.Serialization;

internal sealed partial class JsonEntityIdConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.GetInterfaces().Any(type =>
            type is { IsGenericType: true } &&
            type.GetGenericTypeDefinition() == typeof(IEntityId<,>));

    public override JsonConverter? CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var interfaceType = typeToConvert.GetInterfaces().First(type =>
            type is { IsGenericType: true } &&
            type.GetGenericTypeDefinition() == typeof(IEntityId<,>));

        var valueType = interfaceType.GetGenericArguments()[1];

        var converterType = typeof(EntityIdConverter<,>).MakeGenericType(
            typeToConvert,
            valueType);

        return (JsonConverter?) Activator.CreateInstance(converterType);
    }
}
