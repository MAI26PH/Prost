using Prost.Api.Common.Attributes;
using Prost.Api.Common.Services;
using System.Security.Cryptography;
using System.Text;

namespace Prost.Api.Features.Authentication.Services;

[Service<ITokenHasher>(ServiceLifetime.Singleton)]
internal sealed class TokenHasher : ITokenHasher
{
    public string Hash(string token) =>
        Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(token)));
}
