using Prost.Api.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Prost.Api.Features.Authentication.Options;

[Options("RefreshToken")]
internal sealed class RefreshTokenOptions
{
    [Range(1, int.MaxValue)]
    public int ExpirationInDays { get; init; } = 30;
}
