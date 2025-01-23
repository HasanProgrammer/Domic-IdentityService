using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Events;

namespace Domic.UseCase.UserUseCase.Events;

public class InActiveUserConsumerEventBusHandler(IUserQueryRepository userQueryRepository,
    IExternalDistributedCache externalDistributedCache, ISerializer serializer
) : IConsumerEventBusHandler<UserInActived>
{
    private string _currentUserToken;

    public Task BeforeHandleAsync(UserInActived @event, CancellationToken cancellationToken) => Task.CompletedTask;

    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(UserInActived @event, CancellationToken cancellationToken)
    {
        var targetUser = await userQueryRepository.FindByIdAsync(@event.Id, cancellationToken);

        _currentUserToken = targetUser.Token;

        targetUser.IsActive              = IsActive.InActive;
        targetUser.UpdatedBy             = @event.UpdatedBy;
        targetUser.UpdatedRole           = @event.UpdatedRole;
        targetUser.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
        targetUser.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;

        await userQueryRepository.ChangeAsync(targetUser, cancellationToken);
    }

    public async Task AfterHandleAsync(UserInActived @event, CancellationToken cancellationToken)
    {
        //todo: shoud be used [Polly] for retry perform below action! ( action = update cache auth )
            
        var blackListAuth = await externalDistributedCache.GetCacheValueAsync("BlackList-Auth", cancellationToken);
            
        var tokens = new List<string>();

        if (blackListAuth is not null)
            tokens = serializer.DeSerialize<List<string>>(blackListAuth);

        tokens.Add(_currentUserToken);
            
        await externalDistributedCache.SetCacheValueAsync(
            new KeyValuePair<string, string>("BlackList-Auth", serializer.Serialize(tokens)),
            cancellationToken: cancellationToken
        );
    }
}