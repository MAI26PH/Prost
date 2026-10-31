namespace Prost.Api.Common.Services;

internal interface ITokenHasher
{
    string Hash(string token);
}
