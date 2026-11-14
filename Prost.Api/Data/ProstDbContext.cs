using Microsoft.EntityFrameworkCore;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Data;

internal sealed class ProstDbContext(
    DbContextOptions<ProstDbContext> options
) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<PasswordCredential> PasswordCredentials => Set<PasswordCredential>();
    public DbSet<TokenFamily> TokenFamilies => Set<TokenFamily>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProstDbContext).Assembly);
    }
}
