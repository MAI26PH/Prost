using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Timestamps;

internal interface IPublishedAt : ITimestamp
{
    DateTimeOffset Value { get; set; }
}
