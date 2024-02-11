using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.Role.Entities;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Permission.Entities;

public class PermissionQuery : EntityQuery<string>
{
    public string RoleId { get; set; }
    public string Name   { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public RoleQuery Role { get; set; }
    
    public ICollection<PermissionUserQuery> PermissionUsers { get; set; }
}