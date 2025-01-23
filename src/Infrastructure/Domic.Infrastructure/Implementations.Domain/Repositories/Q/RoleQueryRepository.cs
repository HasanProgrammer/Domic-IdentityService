using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class RoleQueryRepository : IRoleQueryRepository
{
    private readonly SQLContext _context;

    public RoleQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class RoleQueryRepository
{
    public Task AddAsync(RoleQuery entity, CancellationToken cancellationToken)
    {
        _context.Roles.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(RoleQuery entity, CancellationToken cancellationToken)
    {
        _context.Roles.Update(entity);

        return Task.CompletedTask;
    }
}

//Query
public partial class RoleQueryRepository
{
    public async Task<RoleQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.Roles.AsNoTracking().FirstOrDefaultAsync(Role => Role.Id.Equals(id), cancellationToken);

    public async Task<RoleQuery> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken)
        => await _context.Roles.AsNoTracking()
                               .Where(Role => Role.Id.Equals(id))
                               .Include(Role => Role.Permissions)
                               .FirstOrDefaultAsync(cancellationToken);
}