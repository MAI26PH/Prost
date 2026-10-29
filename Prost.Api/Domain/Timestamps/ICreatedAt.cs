using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Timestamps;

internal interface ICreatedAt : ITimestamp
{
    DateTimeOffset Value { get; set; }
}
