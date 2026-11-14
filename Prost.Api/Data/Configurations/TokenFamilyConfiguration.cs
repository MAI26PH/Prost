using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.Primitives;

namespace Prost.Api.Data.Configurations;

internal sealed class TokenFamilyConfiguration
    : IEntityTypeConfiguration<TokenFamily>
{
    public void Configure(EntityTypeBuilder<TokenFamily> builder)
    {
        builder.HasKey(tf => tf.Id);

        builder
            .Property(tf => tf.Id)
            .HasConversion(
                id => id.Value,
                value => Id<TokenFamily>.From(value))
            .ValueGeneratedNever();

        builder
            .Property(tf => tf.AccountId)
            .HasConversion(
                id => id.Value,
                value => Id<Account>.From(value));

        builder.HasOne(tf => tf.Account)
            .WithMany(a => a.TokenFamilies)
            .HasForeignKey(tf => tf.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
