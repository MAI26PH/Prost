using Microsoft.EntityFrameworkCore;

namespace Prost.Api.Data;

internal sealed class ProstDbContext(
    DbContextOptions<ProstDbContext> options
) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProstDbContext).Assembly);
    }
}
