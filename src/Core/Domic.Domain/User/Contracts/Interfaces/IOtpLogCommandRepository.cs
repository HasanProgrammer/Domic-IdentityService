using Domic.Domain.User.Entities;
using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.User.Contracts.Interfaces;

public interface IOtpLogCommandRepository : ICommandRepository<OtpLog, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> IsExistOnNotVerifiedAndNotExpiredAsync(string userId, CancellationToken cancellationToken);
}