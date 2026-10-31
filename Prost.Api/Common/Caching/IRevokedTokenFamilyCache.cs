using Prost.Api.Domain.Entities;

namespace Prost.Api.Common.Caching;

internal interface IRevokedTokenFamilyCache
{
    Task RevokeAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default);

    Task<bool> IsRevokedAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default);
}
