using Microsoft.EntityFrameworkCore;

namespace Prost.Api.Data;

public static class DataDependencyInjection
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProstDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException(
                    "Connection string 'Postgres' is not configured.")));

        return services;
    }
}
