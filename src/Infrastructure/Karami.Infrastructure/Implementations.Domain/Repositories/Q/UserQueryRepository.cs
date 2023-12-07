using Karami.Domain.User.Contracts.Interfaces;
using Karami.Domain.User.Entities;
using Karami.Persistence.Contexts;
using Karami.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class UserQueryRepository : IUserQueryRepository
{
    private readonly SQLContext _context;

    public UserQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class UserQueryRepository
{
    public void Add(UserQuery entity) => _context.Users.Add(entity);

    public void Change(UserQuery entity) => _context.Users.Update(entity);
}

//Query
public partial class UserQueryRepository
{
    public async Task<UserQuery> FindByIdAsync(object id, CancellationToken cancellationToken) 
        => await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(id), cancellationToken);

    public async Task<UserQuery> FindByPasswordAsync(string password, CancellationToken cancellationToken)
        => await _context.Users.FirstOrDefaultAsync(user => user.Password.Equals(password), cancellationToken);

    public async Task<UserQuery> FindByUsernameEagerLoadingAsync(string username, CancellationToken cancellationToken) 
        => await _context.Users.AsNoTracking()
                               .Where(user => user.Username.Equals(username))
                               .Include(user => user.RoleUsers)
                               .ThenInclude(ru => ru.Role)
                               .Include(user => user.PermissionUsers)
                               .ThenInclude(pu => pu.Permission)
                               .FirstOrDefaultAsync(cancellationToken);
}