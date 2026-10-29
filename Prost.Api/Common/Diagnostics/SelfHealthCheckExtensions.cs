using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Prost.Api.Common.Diagnostics;

internal static class SelfHealthCheckExtensions
{
    public static IHealthChecksBuilder AddSelfHealthCheck(
        this IHealthChecksBuilder builder) =>
        builder.AddCheck(
            name: "self",
            () => HealthCheckResult.Healthy(),
            tags: ["live"]);

    public static IEndpointRouteBuilder MapSelfHealthCheck(
        this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks(
           "/health/live",
           new HealthCheckOptions
           {
               Predicate = check => check.Tags.Contains("live")
           });

        return builder;
    }
}
