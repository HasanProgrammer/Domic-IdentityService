using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.OtpVerification;

public class OtpVerificationCommand : ICommand<string>
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}