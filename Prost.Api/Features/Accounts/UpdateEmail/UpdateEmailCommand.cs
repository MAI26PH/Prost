using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Accounts;

internal sealed record UpdateEmailCommand(
    AccountId AccountId,
    string Email
) : IRequest<ErrorOr<UpdateEmailResponse>>;
