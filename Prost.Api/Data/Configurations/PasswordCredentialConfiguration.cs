using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Data.Configurations;

internal sealed class PasswordCredentialConfiguration
    : IEntityTypeConfiguration<PasswordCredential>
{
    public void Configure(EntityTypeBuilder<PasswordCredential> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                value => PasswordCredentialId.From(value));

        builder
            .Property(x => x.AccountId)
            .HasConversion(
                x => x.Value,
                value => AccountId.From(value));

        builder.ComplexProperty(
            x => x.PasswordHash,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(PasswordCredential.PasswordHash))
                .HasMaxLength(PasswordHash.MaxLength)
                .IsRequired());

        builder
            .HasOne(x => x.Account)
            .WithOne(x => x.PasswordCredential)
            .HasForeignKey<PasswordCredential>(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
