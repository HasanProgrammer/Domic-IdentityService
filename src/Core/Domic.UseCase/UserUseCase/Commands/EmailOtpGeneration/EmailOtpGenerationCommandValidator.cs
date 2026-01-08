using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.EmailOtpGeneration;

public class EmailOtpGenerationCommandValidator(
    IUserQueryRepository userQueryRepository,
    IEmailOtpLogCommandRepository otpLogCommandRepository
) : IValidator<EmailOtpGenerationCommand>
{
    public async Task<object> ValidateAsync(EmailOtpGenerationCommand input, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByEmailAsync(input.EmailAddress, cancellationToken);

        if (targetUser is null)
            throw new UseCaseException(
                string.Format("کاربری با پست الکترونیکی {0} در سامانه موجود نمی باشد!", input.EmailAddress)
            );
        
        if (targetUser.IsActive == IsActive.InActive)
            throw new UseCaseException("حساب کاربری شما مسدود شده است!");
        
        if(await otpLogCommandRepository.IsExistOnNotVerifiedAndNotExpiredAsync(targetUser.Id, cancellationToken))
            throw new UseCaseException(
                string.Format("کد یکبار مصرف به پست الکترونیکی {0} ارسال شده است", input.EmailAddress)
            );

        return targetUser;
    }
}