using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Accounts;

internal sealed record UpdateUsernameCommand(
    AccountId AccountId,
    string Username
) : IRequest<ErrorOr<Success>>;
