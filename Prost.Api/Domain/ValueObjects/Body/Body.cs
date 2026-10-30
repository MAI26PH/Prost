using ErrorOr;
using Prost.Api.Domain.Primitives;
using System.Text.Json;

namespace Prost.Api.Domain.ValueObjects;

internal sealed record Body : ValueObject<JsonElement>
{
    private Body(JsonElement value) : base(value) { }

    public static ErrorOr<Body> Create(JsonElement value)
    {
        if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return BodyErrors.Empty;
        }
        if (value.ValueKind is not JsonValueKind.Object)
        {
            return BodyErrors.Unsupported;
        }
        return new Body(value.Clone());
    }
}
