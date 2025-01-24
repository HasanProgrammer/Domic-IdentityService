using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

public class OtpLogCommandRepository(SQLContext context) : IOtpLogCommandRepository
{
    public Task<bool> IsExistOnNotVerifiedAndNotExpiredAsync(string userId, CancellationToken cancellationToken)
        => context.OtpLogs.AnyAsync(ol => 
            ol.UserId == userId && ol.ExpiredAt >= DateTime.UtcNow && ol.IsVerified == false, cancellationToken
        );

    public Task AddAsync(OtpLog entity, CancellationToken cancellationToken)
    {
        context.OtpLogs.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(OtpLog entity, CancellationToken cancellationToken)
    {
        context.OtpLogs.Update(entity);

        return Task.CompletedTask;
    }
}