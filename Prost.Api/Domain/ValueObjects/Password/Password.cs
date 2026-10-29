using ErrorOr;
using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.ValueObjects;

internal sealed record Password : ValueObject<string>
{
    public const int MinLength = 8;
    public const int MaxLength = 64;

    private Password(string value) : base(value) { }

    public static ErrorOr<Password> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return PasswordErrors.Empty;
        }
        var errors = new List<Error>();

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            errors.Add(PasswordErrors.InvalidLength);
        }
        if (value.Any(char.IsWhiteSpace))
        {
            errors.Add(PasswordErrors.WhiteSpace);
        }
        if (errors.Count > 0) return errors;

        return new Password(value);
    }
}
