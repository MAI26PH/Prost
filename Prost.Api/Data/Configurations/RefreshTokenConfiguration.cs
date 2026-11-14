using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.Primitives;

namespace Prost.Api.Data.Configurations;

internal sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder
            .Property(rt => rt.Id)
            .HasConversion(
                id => id.Value,
                value => Id<RefreshToken>.From(value))
            .ValueGeneratedNever();

        builder
            .Property(rt => rt.TokenFamilyId)
            .HasConversion(
                id => id.Value,
                value => Id<TokenFamily>.From(value));

        builder
            .Property(rt => rt.TokenHash)
            .HasMaxLength(128)
            .IsRequired();

        builder
            .HasOne(rt => rt.TokenFamily)
            .WithMany(tf => tf.RefreshTokens)
            .HasForeignKey(rt => rt.TokenFamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(rt => rt.TokenHash)
            .IsUnique();

        builder
            .Property(rt => rt.RowVersion)
            .IsRowVersion();
    }
}
