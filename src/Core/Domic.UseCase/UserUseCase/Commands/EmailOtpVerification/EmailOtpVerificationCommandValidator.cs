using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.EmailOtpVerification;

public class OtpVerificationCommandValidator(IUserQueryRepository userQueryRepository,
    IEmailOtpLogCommandRepository otpLogCommandRepository
) : IValidator<OtpVerificationCommand>
{
    public async Task<object> ValidateAsync(OtpVerificationCommand input, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByEmailEagerLoadingAsync(input.EmailAddress, cancellationToken);

        if (targetUser is null)
            throw new UseCaseException(
                string.Format("کاربری با پست الکترونیکی {0} در سامانه موجود نمی باشد!", input.EmailAddress)
            );
        
        var targetOtpLog =
            await otpLogCommandRepository.FindLastOneOnNotVerifiedAndNotExpiredAsync(targetUser.Id, input.EmailCode,
                cancellationToken
            );
        
        if(targetOtpLog is null)
            throw new UseCaseException("کد یکبار مصرف معتبر نمی باشد!");

        return targetOtpLog;
    }
}