using Domic.Domain.Permission.Entities;
using Domic.Domain.RoleUser.Entities;
using Domic.Core.Domain.Contracts.Abstracts;

#pragma warning disable CS0649

namespace Domic.Domain.Role.Entities;

public class RoleQuery : EntityQuery<string>
{
    public string Name { get; set; }

    /*---------------------------------------------------------------*/
    
    //Relations
    
    public ICollection<RoleUserQuery> RoleUsers     { get; set; }
    public ICollection<PermissionQuery> Permissions { get; set; }
}