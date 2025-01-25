using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.OtpVerification;

public class OtpVerificationCommandValidator(IUserQueryRepository userQueryRepository,
    IOtpLogCommandRepository otpLogCommandRepository
) : IValidator<OtpVerificationCommand>
{
    public async Task<object> ValidateAsync(OtpVerificationCommand input, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByPhoneNumberEagerLoadingAsync(input.PhoneNumber, cancellationToken);

        if (targetUser is null)
            throw new UseCaseException(
                string.Format("کاربری با شماره تماس {0} در سامانه موجود نمی باشد!", input.PhoneNumber)
            );
        
        var targetOtpLog =
            await otpLogCommandRepository.FindLastOneOnNotVerifiedAndNotExpiredAsync(targetUser.Id, input.Code, cancellationToken);
        
        if(targetOtpLog is null)
            throw new UseCaseException("کد یکبار مصرف معتبر نمی باشد!");

        return targetOtpLog;
    }
}