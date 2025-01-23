using Domic.Domain.Role.Entities;
using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Role.Contracts.Interfaces;

public interface IRoleQueryRepository : IQueryRepository<RoleQuery, string>;