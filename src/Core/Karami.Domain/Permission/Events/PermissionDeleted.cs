using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Karami.Domain.Permission.Events;

[MessageBroker(Queue = Broker.Auth_Permission_Queue)]
public class PermissionDeleted : UpdateDomainEvent<string>
{
}