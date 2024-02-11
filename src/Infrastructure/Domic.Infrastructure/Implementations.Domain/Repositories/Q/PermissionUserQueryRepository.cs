using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class PermissionUserQueryRepository : IPermissionUserQueryRepository
{
    private readonly SQLContext _context;
    
    public PermissionUserQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class PermissionUserQueryRepository
{
    public void Add(PermissionUserQuery entity) => _context.PermissionUsers.Add(entity);

    public void Remove(PermissionUserQuery entity) => _context.PermissionUsers.Remove(entity);

    public void RemoveRange(IEnumerable<PermissionUserQuery> entities) => _context.PermissionUsers.RemoveRange(entities);
}

//Query
public partial class PermissionUserQueryRepository
{
    public async Task<PermissionUserQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
        => await _context.PermissionUsers.FirstOrDefaultAsync(pu => pu.Id.Equals(id), cancellationToken);

    public async Task<IEnumerable<PermissionUserQuery>> FindAllByUserIdAsync(string userId,
        CancellationToken cancellationToken
    ) => await _context.PermissionUsers.Where(pu => pu.UserId.Equals(userId)).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<PermissionUserQuery>> FindAllByPermissionIdAsync(string permissionId,
        CancellationToken cancellationToken
    ) => await _context.PermissionUsers.Where(pu => pu.PermissionId.Equals(permissionId))
                                       .ToListAsync(cancellationToken);
}