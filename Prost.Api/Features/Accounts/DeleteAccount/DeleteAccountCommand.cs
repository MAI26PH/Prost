using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Accounts;

internal sealed record DeleteAccountCommand(
    AccountId AccountId,
    TokenFamilyId TokenFamilyId
) : IRequest<ErrorOr<Success>>;
