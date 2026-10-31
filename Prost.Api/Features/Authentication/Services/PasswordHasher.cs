using Prost.Api.Common.Attributes;
using Prost.Api.Common.Services;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace Prost.Api.Features.Authentication.Services;

[Service<IPasswordHasher>(ServiceLifetime.Singleton)]
internal sealed class PasswordHasher : IPasswordHasher
{
    private static readonly string randomHash =
        BCrypt.Net.BCrypt.EnhancedHashPassword(
            Base64Url.EncodeToString(
                RandomNumberGenerator.GetBytes(32)),
            BCrypt.Net.HashType.SHA384);

    public string Hash(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(
            password,
            BCrypt.Net.HashType.SHA384);

    public bool Verify(
        string password,
        string? passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(
            password,
            !string.IsNullOrWhiteSpace(passwordHash)
                ? passwordHash
                : randomHash,
            BCrypt.Net.HashType.SHA384);
}
