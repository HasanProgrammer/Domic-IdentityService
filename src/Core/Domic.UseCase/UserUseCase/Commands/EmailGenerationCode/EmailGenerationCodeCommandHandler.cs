#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;

namespace Domic.UseCase.UserUseCase.Commands.EmailGenerationCode;

public class EmailGenerationCommandHandler(IOtpLogCommandRepository otpLogCommandRepository,
    IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, ISerializer serializer
) : ICommandHandler<EmailGenerationCodeCommand, bool>
{
    private readonly object _validationResult;
    
    public Task BeforeHandleAsync(EmailGenerationCodeCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<bool> HandleAsync(EmailGenerationCodeCommand command, CancellationToken cancellationToken)
    {
        var targetUser = _validationResult as UserQuery;

        var digitCode = Random.Shared.Next(1000, 9999).ToString();

        var newOtpLog = new OtpLog(globalUniqueIdGenerator, dateTime, serializer, targetUser.Id,
            targetUser.PhoneNumber, digitCode, targetUser.RoleUsers.Select(ru => ru.Role.Name).ToList()
        );

        await otpLogCommandRepository.AddAsync(newOtpLog, cancellationToken);

        return true;
    }

    public Task AfterHandleAsync(EmailGenerationCodeCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}