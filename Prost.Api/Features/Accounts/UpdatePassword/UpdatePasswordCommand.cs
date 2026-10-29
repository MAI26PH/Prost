using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Accounts;

internal sealed record UpdatePasswordCommand(
    AccountId AccountId,
    string Password
) : IRequest<ErrorOr<Success>>;
