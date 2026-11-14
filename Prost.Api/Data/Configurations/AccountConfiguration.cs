using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Data.Configurations;

internal sealed class AccountConfiguration :
    IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder
            .Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => Id<Account>.From(value))
            .ValueGeneratedNever();

        builder.ComplexProperty(
            a => a.Email,
            builder => builder
                .Property(e => e.Value)
                .HasColumnName(nameof(Account.Email))
                .HasMaxLength(Email.MaxLength)
                .IsRequired());

        builder
            .HasIndex(a => a.Email.Value)
            .IsUnique();

        builder
            .Property(a => a.RowVersion)
            .IsRowVersion();
    }
}
