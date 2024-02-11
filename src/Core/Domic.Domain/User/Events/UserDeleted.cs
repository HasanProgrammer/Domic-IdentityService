using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Events;

[MessageBroker(Queue = Broker.Auth_User_Queue)]
public class UserDeleted : UpdateDomainEvent<string>
{
    
}