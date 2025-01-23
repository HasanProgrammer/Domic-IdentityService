using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Events;

namespace Domic.UseCase.PermissionUseCase.Events;

public class UpdatePermissionConsumerEventBus : IConsumerEventBusHandler<PermissionUpdated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public UpdatePermissionConsumerEventBus(IPermissionQueryRepository permissionQueryRepository)
        => _permissionQueryRepository = permissionQueryRepository;

    public Task BeforeHandleAsync(PermissionUpdated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(PermissionUpdated @event, CancellationToken cancellationToken)
    {
        var targetPermission = await _permissionQueryRepository.FindByIdAsync(@event.Id, cancellationToken);
            
        if (targetPermission is not null) //replication management
        {
            targetPermission.RoleId                = @event.RoleId;
            targetPermission.Name                  = @event.Name;
            targetPermission.UpdatedBy             = @event.UpdatedBy;
            targetPermission.UpdatedRole           = @event.UpdatedRole;
            targetPermission.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
            targetPermission.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
        
            await _permissionQueryRepository.ChangeAsync(targetPermission, cancellationToken);
        }
    }

    public Task AfterHandleAsync(PermissionUpdated @event, CancellationToken cancellationToken)
        => Task.CompletedTask;
}