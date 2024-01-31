using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Permission.Entities;
using Karami.Domain.Permission.Events;

namespace Karami.UseCase.PermissionUseCase.Events;

public class CreatePermissionConsumerEventBus : IConsumerEventBusHandler<PermissionCreated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public CreatePermissionConsumerEventBus(IPermissionQueryRepository permissionQueryRepository)
        => _permissionQueryRepository = permissionQueryRepository;
    
    [WithTransaction]
    public void Handle(PermissionCreated @event)
    {
        var targetPermission = _permissionQueryRepository.FindByIdAsync(@event.Id, default).Result;
        
        if (targetPermission is null) //Replication management
        {
            var newPermission = new PermissionQuery {
                Id          = @event.Id          ,
                CreatedBy   = @event.CreatedBy   ,
                CreatedRole = @event.CreatedRole ,
                RoleId      = @event.RoleId      ,
                Name        = @event.Name        ,
                CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate,
                CreatedAt_PersianDate = @event.CreatedAt_PersianDate
            };
        
            _permissionQueryRepository.Add(newPermission);
        }
    }
}