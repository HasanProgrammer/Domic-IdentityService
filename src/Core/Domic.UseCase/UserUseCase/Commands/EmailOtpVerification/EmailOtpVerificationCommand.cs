using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.EmailOtpVerification;

public class OtpVerificationCommand : ICommand<string>
{
    public string EmailAddress { get; set; }
    public string EmailCode { get; set; }
}