using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

public class EmailOtpLogCommandRepository(SQLContext context) : IEmailOtpLogCommandRepository
{
    public Task<bool> IsExistOnNotVerifiedAndNotExpiredAsync(string userId, CancellationToken cancellationToken)
        => context.EmailOtpLogs.AnyAsync(ol =>
            ol.UserId == userId && ol.ExpiredAt >= DateTime.UtcNow && ol.IsVerified == false, cancellationToken
        );

    public Task<EmailOtpLog> FindLastOneOnNotVerifiedAndNotExpiredAsync(string userId, string code, 
        CancellationToken cancellationToken
    ) => context.EmailOtpLogs.AsNoTracking()
                             .Where(ol => ol.UserId == userId && ol.IsVerified == false && ol.ExpiredAt >= DateTime.UtcNow)
                             .OrderByDescending(ol => ol.ExpiredAt)
                             .FirstOrDefaultAsync(ol => ol.Code == code, cancellationToken);

    public Task AddAsync(EmailOtpLog entity, CancellationToken cancellationToken)
    {
        context.EmailOtpLogs.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(EmailOtpLog entity, CancellationToken cancellationToken)
    {
        context.EmailOtpLogs.Update(entity);

        return Task.CompletedTask;
    }
}