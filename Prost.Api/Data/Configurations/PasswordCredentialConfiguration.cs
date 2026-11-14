using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.Primitives;

namespace Prost.Api.Data.Configurations;

internal sealed class PasswordCredentialConfiguration
    : IEntityTypeConfiguration<PasswordCredential>
{
    public void Configure(EntityTypeBuilder<PasswordCredential> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder
            .Property(pc => pc.Id)
            .HasConversion(
                id => id.Value,
                value => Id<PasswordCredential>.From(value))
            .ValueGeneratedNever();

        builder
            .Property(pc => pc.AccountId)
            .HasConversion(
                id => id.Value,
                value => Id<Account>.From(value));

        builder
            .Property(pc => pc.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .HasOne(pc => pc.Account)
            .WithOne(a => a.PasswordCredential)
            .HasForeignKey<PasswordCredential>(pc => pc.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(pc => pc.RowVersion)
            .IsRowVersion();
    }
}
