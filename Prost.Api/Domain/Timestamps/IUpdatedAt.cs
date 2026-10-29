using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Timestamps;

internal interface IUpdatedAt : ITimestamp
{
    DateTimeOffset? Value { get; set; }
}
