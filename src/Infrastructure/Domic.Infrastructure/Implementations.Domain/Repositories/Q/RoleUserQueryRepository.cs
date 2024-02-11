using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class RoleUserQueryRepository : IRoleUserQueryRepository
{
    private readonly SQLContext _context;

    public RoleUserQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class RoleUserQueryRepository
{
    public void Add(RoleUserQuery entity) => _context.RoleUsers.Add(entity);

    public void Remove(RoleUserQuery entity) => _context.RoleUsers.Remove(entity);

    public void RemoveRange(IEnumerable<RoleUserQuery> entities) => _context.RoleUsers.RemoveRange(entities);
}

//Query
public partial class RoleUserQueryRepository
{
    public async Task<RoleUserQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.RoleUsers.FirstOrDefaultAsync(ru => ru.Id.Equals(id), cancellationToken);

    public async Task<IEnumerable<RoleUserQuery>> FindAllByUserIdAsync(string userId, 
        CancellationToken cancellationToken
    ) => await _context.RoleUsers.Where(ru => ru.UserId.Equals(userId)).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<RoleUserQuery>> FindAllByRoleIdAsync(string roleId, 
        CancellationToken cancellationToken
    ) => await _context.RoleUsers.Where(ru => ru.RoleId.Equals(roleId)).ToListAsync(cancellationToken);
}