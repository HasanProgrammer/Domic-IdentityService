using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;

namespace Domic.UseCase.UserUseCase.Commands.SignIn;

public class SignInCommandValidator : IValidator<SignInCommand>
{
    public Task<object> ValidateAsync(SignInCommand input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.Username) || string.IsNullOrWhiteSpace(input.Password))
            throw new UseCaseException("نام کاربری و رمز عبور الزامی می باشد!");

        return Task.FromResult(default(object));
    }
}