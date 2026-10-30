using EFCore.ComplexIndexes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using System.Globalization;

namespace Prost.Api.Data.Configurations;

internal sealed class PublishedPostConfiguration
    : IEntityTypeConfiguration<PublishedPost>
{
    public void Configure(EntityTypeBuilder<PublishedPost> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                value => PublishedPostId.From(value));

        builder
            .Property(x => x.AccountId)
            .HasConversion(
                x => x.Value,
                value => AccountId.From(value));

        builder
            .Property(x => x.PostId)
            .HasConversion(
                x => x.Value,
                value => PostId.From(value));

        builder.ComplexProperty(
           x => x.Title,
           builder => builder
               .Property(x => x.Value)
               .HasColumnName(nameof(PublishedPost.Title))
               .HasMaxLength(Title.MaxLength)
               .IsRequired());

        builder.ComplexProperty(
           x => x.Slug,
           builder => builder
               .Property(x => x.Value)
               .HasColumnName(nameof(PublishedPost.Slug))
               .HasMaxLength(Slug.MaxLength)
               .IsRequired());

        builder.ComplexProperty(
           x => x.Body,
           builder => builder
               .Property(x => x.Value)
               .HasColumnName(nameof(PublishedPost.Body))
               .HasColumnType("jsonb")
               .IsRequired());

        builder
            .Property(x => x.UpdatedOrPublishedAt)
            .HasComputedColumnSql(
                string.Format(
                    CultureInfo.InvariantCulture,
                    @"COALESCE(""{0}"", ""{1}"")",
                    nameof(PublishedPost.UpdatedAt),
                    nameof(PublishedPost.PublishedAt)),
                stored: true);

        builder
            .HasOne(x => x.Account)
            .WithMany(x => x.PublishedPosts)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Post)
            .WithOne(x => x.PublishedPost)
            .HasForeignKey<PublishedPost>(x => x.PostId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasComplexIndex(
            x => x.Slug.Value,
            isUnique: true);

        builder
            .HasIndex(x => new { x.UpdatedOrPublishedAt, x.Id })
            .IsDescending(true, true);

        builder
            .Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
