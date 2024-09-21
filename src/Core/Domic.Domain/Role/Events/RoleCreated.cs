using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Role.Events;

[EventConfig(Queue = Broker.Auth_Role_Queue)]
public class RoleCreated : CreateDomainEvent<string>
{
    public string Name { get; init; }
}