using Domic.Domain.User.Entities;
using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.User.Contracts.Interfaces;

public interface IEmailOtpLogCommandRepository : ICommandRepository<EmailOtpLog, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> IsExistOnNotVerifiedAndNotExpiredAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<EmailOtpLog> FindLastOneOnNotVerifiedAndNotExpiredAsync(string userId, string code,
        CancellationToken cancellationToken
    );
}