using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Data.Configurations;

internal sealed class TokenFamilyConfiguration
    : IEntityTypeConfiguration<TokenFamily>
{
    public void Configure(EntityTypeBuilder<TokenFamily> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                value => TokenFamilyId.From(value));

        builder
            .Property(x => x.AccountId)
            .HasConversion(
                x => x.Value,
                value => AccountId.From(value));

        builder.HasOne(x => x.Account)
            .WithMany(x => x.TokenFamilies)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
