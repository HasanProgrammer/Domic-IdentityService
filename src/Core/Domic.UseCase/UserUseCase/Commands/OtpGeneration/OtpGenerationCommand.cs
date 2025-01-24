using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.OtpGeneration;

public class OtpGenerationCommand : ICommand<string>
{
    public string PhoneNumber { get; set; }
}