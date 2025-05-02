using Domic.Core.Common.ClassEnums;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Events;
using Domic.Domain.PermissionUser.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Events;

public class DeletePermissionConsumerEventBus : IConsumerEventBusHandler<PermissionDeleted>
{
    private readonly IPermissionQueryRepository     _permissionQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;

    public DeletePermissionConsumerEventBus(IPermissionQueryRepository permissionQueryRepository, 
        IPermissionUserQueryRepository permissionUserQueryRepository
    )
    {
        _permissionQueryRepository     = permissionQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
    }

    public Task BeforeHandleAsync(PermissionDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(PermissionDeleted @event, CancellationToken cancellationToken)
    {
        var targetPermission = await _permissionQueryRepository.FindByIdAsync(@event.Id, cancellationToken);
            
        if (targetPermission is not null) //replication management
        {
            #region SoftDeletePermission

            targetPermission.IsDeleted             = IsDeleted.Delete;
            targetPermission.UpdatedBy             = @event.UpdatedBy;
            targetPermission.UpdatedRole           = @event.UpdatedRole;
            targetPermission.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
            targetPermission.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
        
            await _permissionQueryRepository.ChangeAsync(targetPermission, cancellationToken);

            #endregion

            #region HardDelete PermissionUser

            var permissionUsers = await _permissionUserQueryRepository.FindAllByPermissionIdAsync(@event.Id, cancellationToken);

            await _permissionUserQueryRepository.RemoveRangeAsync(permissionUsers, cancellationToken);

            #endregion
        }
    }

    public Task AfterHandleAsync(PermissionDeleted @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}