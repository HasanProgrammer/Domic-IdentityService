using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.OtpGeneration;

public class OtpGenerationCommandValidator(IUserQueryRepository userQueryRepository,
    IOtpLogCommandRepository otpLogCommandRepository
) : IValidator<OtpGenerationCommand>
{
    public async Task<object> ValidateAsync(OtpGenerationCommand input, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByPhoneNumberEagerLoadingAsync(input.PhoneNumber, cancellationToken);

        if (targetUser is null)
            throw new UseCaseException(
                string.Format("کاربری با شماره تماس {0} در سامانه موجود نمی باشد!", input.PhoneNumber)
            );
        
        if(await otpLogCommandRepository.IsExistOnNotVerifiedAndNotExpiredAsync(targetUser.Id, cancellationToken))
            throw new UseCaseException(
                string.Format("کد یکبار مصرف به شماره {0} ارسال شده است", input.PhoneNumber)
            );

        return targetUser;
    }
}