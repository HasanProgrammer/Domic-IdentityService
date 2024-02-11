using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Domain.Role.Events;

namespace Domic.UseCase.RoleUseCase.Events;

public class CreateRoleConsumerEventBusHandler : IConsumerEventBusHandler<RoleCreated>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public CreateRoleConsumerEventBusHandler(IRoleQueryRepository roleQueryRepository) 
        => _roleQueryRepository = roleQueryRepository;
    
    [WithTransaction]
    public void Handle(RoleCreated @event)
    {
        var targetRole = _roleQueryRepository.FindByIdAsync(@event.Id, default).Result;
        
        if (targetRole is null) //Replication management
        {
            var newRole = new RoleQuery {
                Id          = @event.Id,
                CreatedBy   = @event.CreatedBy,
                CreatedRole = @event.CreatedRole,
                Name        = @event.Name,
                CreatedAt_EnglishDate = @event.CreatedAt_EnglishDate,
                CreatedAt_PersianDate = @event.CreatedAt_PersianDate
            };

            _roleQueryRepository.Add(newRole);
        }
    }
}