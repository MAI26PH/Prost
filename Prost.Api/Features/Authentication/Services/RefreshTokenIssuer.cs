using Prost.Api.Common.Attributes;
using Prost.Api.Common.Services;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace Prost.Api.Features.Authentication.Services;

[Service<IRefreshTokenIssuer>(ServiceLifetime.Singleton)]
internal sealed class RefreshTokenIssuer : IRefreshTokenIssuer
{
    public string Issue() =>
        Base64Url.EncodeToString(
            RandomNumberGenerator.GetBytes(32));
}
