using Domic.Domain.Permission.Entities;
using Karami.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Permission.Contracts.Interfaces;

public interface IPermissionQueryRepository : IQueryRepository<PermissionQuery, string>
{
    public Task<IEnumerable<PermissionQuery>> FindByRoleIdAsync(string roleId) => throw new NotImplementedException();
}