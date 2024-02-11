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
    
    [WithTransaction]
    public void Handle(PermissionUpdated @event)
    {
        var targetPermission = _permissionQueryRepository.FindByIdAsync(@event.Id, default).GetAwaiter().GetResult();
            
        if (targetPermission is not null) //Replication management
        {
            targetPermission.RoleId      = @event.RoleId;
            targetPermission.UpdatedBy   = @event.UpdatedBy;
            targetPermission.UpdatedRole = @event.UpdatedRole;
            targetPermission.Name        = @event.Name;
            targetPermission.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
            targetPermission.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
        
            _permissionQueryRepository.Change(targetPermission);
        }
    }
}