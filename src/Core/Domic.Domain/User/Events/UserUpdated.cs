using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Events;

[EventConfig(Queue = Broker.Auth_User_Queue)]
public class UserUpdated : UpdateDomainEvent<string>
{
    public string FirstName                { get; init; }
    public string LastName                 { get; init; }
    public string Username                 { get; init; }
    public string Password                 { get; init; }
    public bool IsActive                   { get; init; }
    public IEnumerable<string> Roles       { get; init; }
    public IEnumerable<string> Permissions { get; init; }
}