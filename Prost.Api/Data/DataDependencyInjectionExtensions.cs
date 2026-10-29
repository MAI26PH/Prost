using Microsoft.EntityFrameworkCore;
using Prost.Api.Data.Interceptors;
using System.Text.Json;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Prost.Api.Data;

internal static class DataDependencyInjectionExtensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProstDbContext>((sp, options) => options
            .UseNpgsql(
                configuration.GetConnectionString("Postgres")
                    ?? throw new InvalidOperationException(
                        "Connection string 'Postgres' is not configured."))
            .AddInterceptors(
                sp.GetRequiredService<TimestampsInterceptor>()));

        services
            .AddFusionCache()
            .WithDefaultEntryOptions(new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(15),
                JitterMaxDuration = TimeSpan.FromSeconds(30),
            })
            .WithSerializer(new FusionCacheSystemTextJsonSerializer(
                new JsonSerializerOptions { IncludeFields = true }));

        return services;
    }
}
