using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Domain.User.Entities;

namespace Karami.Domain.User.Contracts.Interfaces;

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