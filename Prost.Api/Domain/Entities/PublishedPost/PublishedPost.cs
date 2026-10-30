using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Domain.Entities;

internal sealed class PublishedPost :
    Entity<PublishedPostId>, IPublishedAt, IUpdatedAt
{
    public AccountId AccountId { get; }
    public PostId PostId { get; }
    public Title Title { get; private set; }

    public void UpdateTitle(Title title)
    {
        Title = title;
    }

    public Slug Slug { get; }

    public Body Body { get; private set; }

    public void UpdateBody(Body body)
    {
        Body = body;
    }

    public DateTimeOffset PublishedAt { get; private set; }
    DateTimeOffset IPublishedAt.Value
    {
        get => PublishedAt;
        set => PublishedAt = value;
    }
    public DateTimeOffset? UpdatedAt { get; private set; }
    DateTimeOffset? IUpdatedAt.Value
    {
        get => UpdatedAt;
        set => UpdatedAt = value;
    }
    public DateTimeOffset UpdatedOrPublishedAt { get; }

    public Account Account { get; } = null!;
    public Post Post { get; } = null!;

    public uint RowVersion { get; }

    private PublishedPost(PublishedPostId id) : base(id)
    {
        Title = default!;
        Slug = default!;
        Body = default!;
    }

    private PublishedPost(
        PublishedPostId id,
        AccountId accountId,
        PostId postId,
        Title title,
        Slug slug,
        Body body
    ) : base(id)
    {
        AccountId = accountId;
        PostId = postId;
        Title = title;
        Slug = slug;
        Body = body;
    }

    public static PublishedPost Create(
        PublishedPostId id,
        Post post) => new(
            id,
            post.AccountId,
            post.Id,
            post.Title,
            post.Slug,
            post.Body);
}
