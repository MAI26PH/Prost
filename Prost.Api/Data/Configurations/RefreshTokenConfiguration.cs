using EFCore.ComplexIndexes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Data.Configurations;

internal sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                value => RefreshTokenId.From(value));

        builder
            .Property(x => x.TokenFamilyId)
            .HasConversion(
                x => x.Value,
                value => TokenFamilyId.From(value));

        builder.ComplexProperty(
            x => x.TokenHash,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(RefreshToken.TokenHash))
                .HasMaxLength(TokenHash.MaxLength)
                .IsRequired());

        builder
            .HasOne(x => x.TokenFamily)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.TokenFamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasComplexIndex(
            x => x.TokenHash.Value,
            isUnique: true);

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
