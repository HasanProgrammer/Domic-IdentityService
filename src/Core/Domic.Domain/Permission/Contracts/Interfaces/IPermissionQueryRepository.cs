using Domic.Domain.Permission.Entities;
using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Permission.Contracts.Interfaces;

public interface IPermissionQueryRepository : IQueryRepository<PermissionQuery, string>;