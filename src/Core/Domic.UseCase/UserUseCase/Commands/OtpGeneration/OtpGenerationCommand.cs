using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.OtpGeneration;

public class OtpGenerationCommand : ICommand<bool>
{
    public string PhoneNumber { get; set; }
}