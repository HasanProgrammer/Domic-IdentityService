using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "Identity_User_Exchange")]
public class UserPasswordChanged : UpdateDomainEvent<string>
{
    public string NewPassword { get; init; }
}