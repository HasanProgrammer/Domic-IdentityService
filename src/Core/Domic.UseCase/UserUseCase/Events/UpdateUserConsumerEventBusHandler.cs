using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class UpdateUserConsumerEventBusHandler : IConsumerEventBusHandler<UserUpdated>
{
    private readonly IUserQueryRepository           _userQueryRepository;
    private readonly IRoleUserQueryRepository       _roleUserQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;
    private readonly IGlobalUniqueIdGenerator       _globalUniqueIdGenerator;

    public UpdateUserConsumerEventBusHandler(IUserQueryRepository userQueryRepository, 
        IRoleUserQueryRepository roleUserQueryRepository, IPermissionUserQueryRepository permissionUserQueryRepository,
        IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _userQueryRepository           = userQueryRepository;
        _roleUserQueryRepository       = roleUserQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
        _globalUniqueIdGenerator       = globalUniqueIdGenerator;
    }
    
    [WithTransaction]
    public void Handle(UserUpdated @event)
    {
        var targetUser = _userQueryRepository.FindByIdAsync(@event.Id, default).Result;

        targetUser.UpdatedBy   = @event.UpdatedBy;
        targetUser.UpdatedRole = @event.UpdatedRole;
        targetUser.FirstName   = @event.FirstName;
        targetUser.LastName    = @event.LastName;
        targetUser.Username    = @event.Username;
        targetUser.IsActive    = @event.IsActive ? IsActive.Active : IsActive.InActive;
        targetUser.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
        targetUser.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
                
        if(targetUser.Password is not null)
            targetUser.Password = @event.Password.HashAsync(default).Result;
                    
        _userQueryRepository.Change(targetUser);
                    
        _roleUserBuilder(targetUser.Id, @event.Roles, @event.UpdatedBy, @event.UpdatedRole, 
            @event.UpdatedAt_EnglishDate, @event.UpdatedAt_PersianDate
        );
        
        _permissionUserBuilder(targetUser.Id, @event.Permissions, @event.UpdatedBy, @event.UpdatedRole,
            @event.UpdatedAt_EnglishDate, @event.UpdatedAt_PersianDate
        );
    }
    
    /*---------------------------------------------------------------*/
    
    private void _roleUserBuilder(string userId, IEnumerable<string> roleIds, string updatedBy, string updatedRole,
        DateTime englishUpdatedAt, string persianUpdatedAt
    )
    {
        var roleUsers = _roleUserQueryRepository.FindAllByUserIdAsync(userId, default).Result;
        
        //1 . Remove already user roles
        _roleUserQueryRepository.RemoveRange(roleUsers);
        
        //2 . Assign new roles for user
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUserQuery {
                Id          = _globalUniqueIdGenerator.GetRandom(),
                CreatedBy   = updatedBy, 
                CreatedRole = updatedRole,
                UserId      = userId, 
                RoleId      = roleId,
                CreatedAt_EnglishDate = englishUpdatedAt,
                CreatedAt_PersianDate = persianUpdatedAt
            };

            _roleUserQueryRepository.Add(newRoleUser);
        }
    }
    
    private void _permissionUserBuilder(string userId , IEnumerable<string> permissionIds, string updatedBy, 
        string updatedRole, DateTime englishUpdatedAt, string persianUpdatedAt
    )
    {
        var permissionUsers = _permissionUserQueryRepository.FindAllByUserIdAsync(userId, default).Result;
        
        //1 . Remove already user permissions
        _permissionUserQueryRepository.RemoveRange(permissionUsers);
        
        //2 . Assign new permissions for user
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUserQuery {
                Id           = _globalUniqueIdGenerator.GetRandom(),
                CreatedBy    = updatedBy,
                CreatedRole  = updatedRole, 
                UserId       = userId,
                PermissionId = permissionId,
                CreatedAt_EnglishDate = englishUpdatedAt,
                CreatedAt_PersianDate = persianUpdatedAt
            };

            _permissionUserQueryRepository.Add(newPermissionUser);
        }
    }
}