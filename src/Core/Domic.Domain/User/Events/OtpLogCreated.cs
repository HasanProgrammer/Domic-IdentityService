using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "Identity_OtpLog_Exchange")]
public class OtpLogCreated : CreateDomainEvent<string>
{
    public string UserId { get; init; }
    public string Code { get; init; }
    public bool IsVerified { get; init; }
    public DateTime ExpiredAt { get; init; }
}