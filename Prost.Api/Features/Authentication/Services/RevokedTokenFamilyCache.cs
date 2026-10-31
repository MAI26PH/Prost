using Microsoft.Extensions.Options;
using Prost.Api.Common.Attributes;
using Prost.Api.Common.Services;
using Prost.Api.Domain.Entities;
using Prost.Api.Features.Authentication.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Authentication.Services;

[Service<IRevokedTokenFamilyCache>(ServiceLifetime.Singleton)]
internal sealed class RevokedTokenFamilyCache(
    IFusionCache cache,
    IOptions<JwtOptions> options
) : IRevokedTokenFamilyCache
{
    public async Task RevokeAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default)
    {
        await cache.SetAsync(
            $"token-families:{id}:revoked",
            true,
            new FusionCacheEntryOptions(
                TimeSpan.FromMinutes(
                    options.Value.ExpirationInMinutes)),
            cancellationToken);
    }

    public async Task<bool> IsRevokedAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default) =>
        await cache.GetOrDefaultAsync<bool>(
            $"token-families:{id}:revoked",
            token: cancellationToken);
}
