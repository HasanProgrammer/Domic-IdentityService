using Domic.Domain.User.Entities;
using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.User.Contracts.Interfaces;

public interface IUserQueryRepository : IQueryRepository<UserQuery, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<UserQuery> FindByUsernameEagerLoadingAsync(string username, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}