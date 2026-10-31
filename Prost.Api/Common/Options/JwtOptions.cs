using Microsoft.IdentityModel.Tokens;
using Prost.Api.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Prost.Api.Common.Options;

[Options("Jwt")]
internal sealed class JwtOptions
{
    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }

    [Required, MinLength(32)]
    public required string SigningKey { get; init; }

    [Range(1, int.MaxValue)]
    public int ExpirationInMinutes { get; init; } = 15;

    public string Algorithm { get; init; } = SecurityAlgorithms.HmacSha256;
}
