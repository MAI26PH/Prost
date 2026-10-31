using Prost.Api.Common.Attributes;
using Prost.Api.Domain.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Authentication.Services;

internal interface IRevokedTokenFamilyCache
{
    Task SetAsync(
        TokenFamilyId id,
        DateTimeOffset expiresAt,
        CancellationToken cancellationToken = default);

    Task RevokeAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default);

    Task<bool> IsRevokedAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default);
}

[Service<IRevokedTokenFamilyCache>(ServiceLifetime.Singleton)]
internal sealed class RevokedTokenFamilyCache(
    IFusionCache cache,
    TimeProvider time
) : IRevokedTokenFamilyCache
{
    private static string CacheKeyOf(TokenFamilyId id) =>
        $"token-families:{id}:revocation";

    private sealed record CacheEntry(
        bool IsRevoked,
        DateTimeOffset ExpiresAt);

    public async Task SetAsync(
        TokenFamilyId id,
        DateTimeOffset expiresAt,
        CancellationToken cancellationToken = default)
    {
        var key = CacheKeyOf(id);

        var entry = await cache.GetOrDefaultAsync<CacheEntry>(
            key,
            token: cancellationToken);

        if (entry?.IsRevoked == true) return;

        var now = time.GetUtcNow();

        var duration = expiresAt - now;

        if (duration <= TimeSpan.Zero) return;

        await cache.SetAsync(
            key,
            new CacheEntry(false, expiresAt),
            new FusionCacheEntryOptions(duration),
            cancellationToken);
    }

    public async Task RevokeAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default)
    {
        var key = CacheKeyOf(id);

        var entry = await cache.GetOrDefaultAsync<CacheEntry>(
            key,
            token: cancellationToken);

        if (entry is null) return;

        var now = time.GetUtcNow();

        var duration = entry.ExpiresAt - now;

        if (duration <= TimeSpan.Zero) return;

        await cache.SetAsync(
            key,
            entry with { IsRevoked = true },
            new FusionCacheEntryOptions(duration),
            cancellationToken);
    }

    public async Task<bool> IsRevokedAsync(
        TokenFamilyId id,
        CancellationToken cancellationToken = default)
    {
        var entry = await cache.GetOrDefaultAsync<CacheEntry>(
            CacheKeyOf(id),
            token: cancellationToken);

        return entry?.IsRevoked ?? false;
    }
}
