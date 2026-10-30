using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;
using Prost.Api.Domain.ValueObjects;

namespace Prost.Api.Domain.Entities;

internal sealed class Post :
    Entity<PostId>, ICreatedAt, IUpdatedAt
{
    public AccountId AccountId { get; }
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

    public DateTimeOffset CreatedAt { get; private set; }
    DateTimeOffset ICreatedAt.Value
    {
        get => CreatedAt;
        set => CreatedAt = value;
    }
    public DateTimeOffset? UpdatedAt { get; private set; }
    DateTimeOffset? IUpdatedAt.Value
    {
        get => UpdatedAt;
        set => UpdatedAt = value;
    }
    public DateTimeOffset UpdatedOrCreatedAt { get; }

    public Account Account { get; } = null!;

    public uint RowVersion { get; }

    private Post(PostId id) : base(id)
    {
        Title = default!;
        Slug = default!;
        Body = default!;
    }

    private Post(
        PostId id,
        AccountId accountId,
        Title title,
        Slug slug,
        Body body
    ) : base(id)
    {
        AccountId = accountId;
        Title = title;
        Slug = slug;
        Body = body;
    }

    public static Post Create(
        PostId id,
        AccountId accountId,
        Title title,
        Slug slug,
        Body body) => new(
            id,
            accountId,
            title,
            slug,
            body);
}
