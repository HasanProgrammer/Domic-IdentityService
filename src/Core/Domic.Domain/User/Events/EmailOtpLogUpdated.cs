using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "Identity_EmailOtpLog_Exchange")]
public class EmailOtpLogUpdated : UpdateDomainEvent<string>
{
    public string UserId { get; init; }
    public string EmailAddress { get; set; }
    public string MessageContent { get; init; }
    public bool IsVerified { get; init; }
    public DateTime ExpiredAt { get; init; }
}