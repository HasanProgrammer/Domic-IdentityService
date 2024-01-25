using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Karami.Domain.User.Events;

[MessageBroker(Queue = Broker.Auth_User_Queue)]
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