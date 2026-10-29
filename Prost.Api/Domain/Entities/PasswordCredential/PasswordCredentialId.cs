using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.Entities;

internal readonly record struct PasswordCredentialId :
    IEntityId<PasswordCredentialId, Guid>
{
    public Guid Value { get; }

    private PasswordCredentialId(Guid value)
    {
        Value = value;
    }

    public static PasswordCredentialId Create() =>
        new(Guid.CreateVersion7());

    public static PasswordCredentialId From(Guid value) =>
        new(value);

    public override string ToString() =>
        Value.ToString();

    public int CompareTo(PasswordCredentialId other) =>
        Value.CompareTo(other.Value);

    public static bool operator >(PasswordCredentialId left, PasswordCredentialId right) =>
        left.CompareTo(right) > 0;

    public static bool operator <(PasswordCredentialId left, PasswordCredentialId right) =>
        left.CompareTo(right) < 0;

    public static bool operator >=(PasswordCredentialId left, PasswordCredentialId right) =>
        left.CompareTo(right) >= 0;

    public static bool operator <=(PasswordCredentialId left, PasswordCredentialId right) =>
        left.CompareTo(right) <= 0;
}
