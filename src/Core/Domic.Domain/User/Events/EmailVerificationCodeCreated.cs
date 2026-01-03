using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "Identity_EmailVerification_Exchange")]
public class EmailVerificationCodeCreated : CreateDomainEvent<string>
{
    public string UserId { get; init; }
    public string EmailAddress { get; set; }
    public string VerifyCode { get; set; }
    public bool IsVerified { get; init; }
    public DateTime ExpiredAt { get; init; }
}