using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Permission.Events;

[EventConfig(Queue = Broker.Auth_Permission_Queue)]
public class PermissionDeleted : UpdateDomainEvent<string>
{
}