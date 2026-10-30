using EFCore.ComplexIndexes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using System.Globalization;

namespace Prost.Api.Data.Configurations;

internal sealed class PostConfiguration
    : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                value => PostId.From(value));

        builder
            .Property(x => x.AccountId)
            .HasConversion(
                x => x.Value,
                value => AccountId.From(value));

        builder.ComplexProperty(
            x => x.Title,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(Post.Title))
                .HasMaxLength(Title.MaxLength)
                .IsRequired());

        builder.ComplexProperty(
            x => x.Slug,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(Post.Slug))
                .HasMaxLength(Slug.MaxLength)
                .IsRequired());

        builder.ComplexProperty(
            x => x.Body,
            builder => builder
                .Property(x => x.Value)
                .HasColumnName(nameof(Post.Body))
                .HasColumnType("jsonb")
                .IsRequired());

        builder
            .Property(x => x.UpdatedOrCreatedAt)
            .HasComputedColumnSql(
                string.Format(
                    CultureInfo.InvariantCulture,
                    @"COALESCE(""{0}"", ""{1}"")",
                    nameof(Post.UpdatedAt),
                    nameof(Post.CreatedAt)),
                stored: true);

        builder
            .HasOne(x => x.Account)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasComplexIndex(
            x => x.Slug.Value,
            isUnique: true);

        builder
            .HasIndex(x => new { x.UpdatedOrCreatedAt, x.Id })
            .IsDescending(true, true);

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
