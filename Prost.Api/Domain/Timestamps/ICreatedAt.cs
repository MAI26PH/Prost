using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Timestamps;

public interface ICreatedAt : ITimestamp
{
    DateTimeOffset Value { get; set; }
}
