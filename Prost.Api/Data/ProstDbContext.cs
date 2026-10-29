using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Data;

internal sealed class ProstDbContext(
    DbContextOptions<ProstDbContext> options
) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<PasswordCredential> PasswordCredentials => Set<PasswordCredential>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<TokenFamily> TokenFamilies => Set<TokenFamily>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProstDbContext).Assembly);
    }
}
