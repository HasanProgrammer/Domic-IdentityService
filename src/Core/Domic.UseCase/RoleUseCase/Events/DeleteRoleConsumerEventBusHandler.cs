using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Events;
using Domic.Domain.RoleUser.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Events;

public class DeleteRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleDeleted>
{
    private readonly IRoleQueryRepository           _roleQueryRepository;
    private readonly IPermissionQueryRepository     _permissionQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;
    private readonly IRoleUserQueryRepository       _roleUserQueryRepository;

    public DeleteRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository, 
        IPermissionQueryRepository permissionQueryRepository, IRoleUserQueryRepository roleUserQueryRepository,
        IPermissionUserQueryRepository permissionUserQueryRepository
    )
    {
        _roleQueryRepository           = roleQueryRepository;
        _permissionQueryRepository     = permissionQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
        _roleUserQueryRepository       = roleUserQueryRepository;
    }

    public Task BeforeHandleAsync(RoleDeleted @event, CancellationToken cancellationToken) => Task.CompletedTask;

    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(RoleDeleted @event, CancellationToken cancellationToken)
    {
        var targetRole = await _roleQueryRepository.FindByIdEagerLoadingAsync(@event.Id, cancellationToken);
            
        if (targetRole is not null) //Replication management
        {
            #region SoftDeleteRole

            targetRole.UpdatedBy   = @event.UpdatedBy;
            targetRole.UpdatedRole = @event.UpdatedRole;
            targetRole.IsDeleted   = IsDeleted.Delete;
            targetRole.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
            targetRole.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
        
            await _roleQueryRepository.ChangeAsync(targetRole, cancellationToken);

            #endregion
        
            #region SoftDeletePermission

            foreach (var permission in targetRole.Permissions)
            {
                permission.UpdatedBy   = @event.UpdatedBy;
                permission.UpdatedRole = @event.UpdatedRole;
                permission.IsDeleted   = IsDeleted.Delete;
                permission.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
                permission.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
            
                await _permissionQueryRepository.ChangeAsync(permission, cancellationToken);
            }

            #endregion
        
            #region HardDeleteRoleUser

            var roleUsers = await _roleUserQueryRepository.FindAllByRoleIdAsync(@event.Id, cancellationToken);
        
            await _roleUserQueryRepository.RemoveRangeAsync(roleUsers, cancellationToken);

            #endregion
        
            #region HardDeletePermissionUser

            foreach (var permission in targetRole.Permissions)
            {
                var permissionUsers =
                    await _permissionUserQueryRepository.FindAllByPermissionIdAsync(permission.Id, cancellationToken);
        
                await _permissionUserQueryRepository.RemoveRangeAsync(permissionUsers, cancellationToken);
            }

            #endregion
        }
    }

    public Task AfterHandleAsync(RoleDeleted @event, CancellationToken cancellationToken) => Task.CompletedTask;
}