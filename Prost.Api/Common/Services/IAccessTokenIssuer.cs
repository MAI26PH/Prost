using Prost.Api.Domain.Entities;

namespace Prost.Api.Common.Services;

internal interface IAccessTokenIssuer
{
    string Issue(
        AccountId accountId,
        TokenFamilyId tokenFamilyId);
}
