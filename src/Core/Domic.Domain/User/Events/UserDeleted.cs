using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Events;

[MessageBroker(Queue = Broker.Auth_User_Queue)]
public class UserDeleted : UpdateDomainEvent<string>
{
    
}