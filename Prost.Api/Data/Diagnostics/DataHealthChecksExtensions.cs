using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Prost.Api.Data.Diagnostics;

internal static class DataHealthChecksExtensions
{
    public static IHealthChecksBuilder AddDataHealthChecks(
        this IHealthChecksBuilder builder,
        IConfiguration configuration) =>
        builder.AddNpgSql(
            configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException(
                    "Connection string 'Postgres' is not configured."),
            name: "postgres",
            tags: ["ready"]);

    public static IEndpointRouteBuilder MapDataHealthChecks(
        this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks(
            "/health/ready",
            new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready")
            });

        return builder;
    }
}
