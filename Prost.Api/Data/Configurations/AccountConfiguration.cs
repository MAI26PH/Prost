using EFCore.ComplexIndexes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Data.Configurations;

internal sealed class AccountConfiguration
    : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                value => AccountId.From(value));

        builder.ComplexProperty(
            x => x.Username,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(Account.Username))
                .HasMaxLength(Username.MaxLength)
                .IsRequired());

        builder.ComplexProperty(
            x => x.Email,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(Account.Email))
                .HasMaxLength(Email.MaxLength)
                .IsRequired());

        builder.HasComplexIndex(
            x => x.Username.Value,
            isUnique: true);

        builder.HasComplexIndex(
            x => x.Email.Value,
            isUnique: true);

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
