using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

//Config
public partial class UserQueryRepository : IUserQueryRepository
{
    private readonly SQLContext _context;

    public UserQueryRepository(SQLContext context) => _context = context;
}

//Transaction
public partial class UserQueryRepository
{
    public Task AddAsync(UserQuery entity, CancellationToken cancellationToken)
    {
        _context.Users.Add(entity);

        return Task.CompletedTask;
    }

    public Task ChangeAsync(UserQuery entity, CancellationToken cancellationToken)
    {
        _context.Users.Update(entity);

        return Task.CompletedTask;
    }
}

//Query
public partial class UserQueryRepository
{
    public async Task<UserQuery> FindByIdAsync(object id, CancellationToken cancellationToken) 
        => await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(id), cancellationToken);

    public Task<UserQuery> FindByIdEagerLoadingAsync(object id, CancellationToken cancellationToken) 
        => _context.Users.Include(User => User.RoleUsers)
                         .ThenInclude(RoleUser => RoleUser.Role)
                         .Include(User => User.PermissionUsers)
                         .ThenInclude(PermissionUser => PermissionUser.Permission)
                         .ThenInclude(Permission => Permission.Role)
                         .FirstOrDefaultAsync(User => User.Id.Equals(id), cancellationToken);

    public async Task<UserQuery> FindByUsernameEagerLoadingAsync(string username, CancellationToken cancellationToken) 
        => await _context.Users.AsNoTracking()
                               .Where(user => user.Username.Equals(username))
                               .Include(user => user.RoleUsers)
                               .ThenInclude(ru => ru.Role)
                               .Include(user => user.PermissionUsers)
                               .ThenInclude(pu => pu.Permission)
                               .FirstOrDefaultAsync(cancellationToken);

    public Task<UserQuery> FindByPhoneNumberEagerLoadingAsync(string phoneNumber, CancellationToken cancellationToken) 
        => _context.Users.AsNoTracking()
                         .Include(user => user.RoleUsers)
                         .ThenInclude(roleUser => roleUser.Role)
                         .FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber, cancellationToken);

    public Task<UserQuery> FindByEmailAsync(string email, CancellationToken cancellationToken) 
        => _context.Users.AsNoTracking()
                         .FirstOrDefaultAsync(user => user == phoneNumber, cancellationToken);
}