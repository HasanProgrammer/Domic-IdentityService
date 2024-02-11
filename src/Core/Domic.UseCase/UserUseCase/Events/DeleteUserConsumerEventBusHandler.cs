using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class DeleteUserConsumerEventBusHandler : IConsumerEventBusHandler<UserDeleted>
{
    private readonly IQueryUnitOfWork _queryUnitOfWork;

    public DeleteUserConsumerEventBusHandler(IQueryUnitOfWork queryUnitOfWork) => _queryUnitOfWork = queryUnitOfWork;
    
    [WithTransaction]
    public void Handle(UserDeleted @event)
    {
        
    }
}