using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.EmailGenerationCode;

public class EmailGenerationCodeCommand : ICommand<bool>
{
    public string EmailAddress { get; set; }
}