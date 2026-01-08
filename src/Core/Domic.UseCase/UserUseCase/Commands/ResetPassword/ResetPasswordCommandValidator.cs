using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.ResetPassword;

public class SignInCommandValidator(
    IUserQueryRepository userQueryRepository, IEmailOtpLogCommandRepository otpLogCommandRepository
) : IValidator<ResetPasswordCommand>
{
    public async Task<object> ValidateAsync(ResetPasswordCommand input, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByEmailAsync(input.EmailAddress, cancellationToken);

        if (targetUser is null)
            throw new UseCaseException(
                string.Format("کاربری با پست الکترونیکی {0} در سامانه موجود نمی باشد!", input.EmailAddress)
            );
        
        if (targetUser.IsActive == IsActive.InActive)
            throw new UseCaseException("حساب کاربری شما مسدود شده است!");
        
        var targetOtpLog =
            await otpLogCommandRepository.FindLastOneOnNotVerifiedAndNotExpiredAsync(targetUser.Id, input.EmailCode,
                cancellationToken
            );
        
        if(targetOtpLog is null)
            throw new UseCaseException("کد یکبار مصرف معتبر نمی باشد!");

        return targetOtpLog;
    }
}