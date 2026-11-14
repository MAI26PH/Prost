using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Timestamps;

public interface IUpdatedAt : ITimestamp
{
    DateTimeOffset? Value { get; set; }
}
