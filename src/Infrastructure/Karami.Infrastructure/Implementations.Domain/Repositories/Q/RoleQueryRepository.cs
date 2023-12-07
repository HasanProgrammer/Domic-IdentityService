using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Entities;
using Karami.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class RoleQueryRepository : IRoleQueryRepository
{
    private readonly SQLContext _context;

    public RoleQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class RoleQueryRepository
{
    public void Add(RoleQuery entity) => _context.Roles.Add(entity);

    public void Change(RoleQuery entity) => _context.Roles.Update(entity);

    public void Remove(RoleQuery entity) => _context.Roles.Remove(entity);

    public void SoftDelete(RoleQuery entity) => _context.Roles.Update(entity);
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