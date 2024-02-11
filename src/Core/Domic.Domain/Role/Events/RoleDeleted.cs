using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Role.Events;

[MessageBroker(Queue = Broker.Auth_Role_Queue)]
public class RoleDeleted : UpdateDomainEvent<string>
{
    
}