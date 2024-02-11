using Karami.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.SignIn;

public class SignInCommand : ICommand<string>
{
    public string Username { get; set; }
    public string Password { get; set; }
}