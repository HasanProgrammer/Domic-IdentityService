#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

using Domic.Core.Common.ClassEnums;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Entities;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.Extensions;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Domain.User.Events;
using Microsoft.Extensions.Configuration;

namespace Domic.UseCase.UserUseCase.Commands.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly IUserQueryRepository          _userQueryRepository;
    private readonly IDateTime                     _dateTime;
    private readonly IEmailOtpLogCommandRepository _otpLogCommandRepository;
    private readonly IEventCommandRepository       _eventCommandRepository;
    private readonly ISerializer                   _serializer;
    private readonly IConfiguration                _configuration;
    
    private readonly object _validationResult;

    public ResetPasswordCommandHandler(
        IUserQueryRepository userQueryRepository,
        IDateTime dateTime,
        IEmailOtpLogCommandRepository otpLogCommandRepository, 
        IEventCommandRepository eventCommandRepository, 
        ISerializer serializer, 
        IConfiguration configuration
    )
    {
        _userQueryRepository     = userQueryRepository;
        _dateTime                = dateTime;
        _otpLogCommandRepository = otpLogCommandRepository;
        _eventCommandRepository  = eventCommandRepository;
        _serializer              = serializer;
        _configuration           = configuration;
    }

    public Task BeforeHandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction(Type = TransactionType.Query)]
    public async Task<bool> HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var nowDateTime = DateTime.UtcNow;
        var nowPersianDateTime = _dateTime.ToPersianShortDate(nowDateTime);
        
        var targetOtpLog = _validationResult as EmailOtpLog;
        
        targetOtpLog.ChangeVerification(_dateTime, command.EmailAddress, true, targetOtpLog.UserId, targetOtpLog.CreatedRole);

        await _otpLogCommandRepository.ChangeAsync(targetOtpLog, cancellationToken);

        var targetUser =
            await _userQueryRepository.FindByIdEagerLoadingAsync(targetOtpLog.UserId, cancellationToken);
        
        if (targetUser.IsActive == IsActive.InActive)
            throw new UseCaseException("حساب کاربری شما مسدود شده است!");
        
        targetUser.Password = await command.NewPassword.HashAsync(cancellationToken);

        var newEvent = new UserPasswordChanged {
            Id = targetUser.Id,
            NewPassword = command.NewPassword,
            UpdatedBy = targetOtpLog.UserId,
            UpdatedRole = targetOtpLog.UpdatedRole,
            UpdatedAt_EnglishDate = nowDateTime,
            UpdatedAt_PersianDate = nowPersianDateTime
        };
        
        await _eventCommandRepository.ChangeAsync(MapToEventModel(newEvent), cancellationToken);
        await _userQueryRepository.ChangeAsync(targetUser, cancellationToken);

        return true;
    }

    public Task AfterHandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken) => Task.CompletedTask;
    
    /*---------------------------------------------------------------*/
    
    private Event MapToEventModel(UserPasswordChanged userPasswordChanged) 
        => new() {
            Id = userPasswordChanged.Id,
            Service = _configuration.GetValue<string>("NameOfService"),
            Table = "Users",
            Action = "UPDATE",
            Type = nameof(UserPasswordChanged),
            Payload = _serializer.Serialize(userPasswordChanged),
            UpdatedAt_EnglishDate = userPasswordChanged.UpdatedAt_EnglishDate,
            UpdatedAt_PersianDate = userPasswordChanged.UpdatedAt_PersianDate
        };
}