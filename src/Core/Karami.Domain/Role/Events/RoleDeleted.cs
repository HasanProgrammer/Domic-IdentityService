using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Karami.Domain.Role.Events;

[MessageBroker(Queue = Broker.Auth_Role_Queue)]
public class RoleDeleted : DeleteDomainEvent
{
    public string Id { get; set; }
}