using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class PermissionQueryRepository : IPermissionQueryRepository
{
    private readonly SQLContext _context;

    public PermissionQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class PermissionQueryRepository
{
    public Task AddAsync(PermissionQuery entity, CancellationToken cancellationToken)
    {
        _context.Permissions.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(PermissionQuery entity, CancellationToken cancellationToken)
    {
        _context.Permissions.Update(entity);

        return Task.CompletedTask;
    }
}

//Query
public partial class PermissionQueryRepository
{
    public async Task<PermissionQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.Permissions.FirstOrDefaultAsync(Permission => Permission.Id.Equals(id), cancellationToken);
}