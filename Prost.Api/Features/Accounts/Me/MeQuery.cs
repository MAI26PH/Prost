using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Accounts;

internal sealed record MeQuery(
    AccountId AccountId
) : IRequest<ErrorOr<MeResponse>>;
