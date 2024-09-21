using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Permission.Events;

[EventConfig(Queue = Broker.Auth_Permission_Queue)]
public class PermissionCreated : CreateDomainEvent<string>
{
    public string RoleId { get; init; }
    public string Name   { get; init; }
}