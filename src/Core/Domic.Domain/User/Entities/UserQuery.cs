#pragma warning disable CS0649

using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.RoleUser.Entities;
using Karami.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Entities;

public class UserQuery : EntityQuery<string>
{
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public string Username  { get; set; }
    public string Password  { get; set; }

    /*---------------------------------------------------------------*/
    
    //Relations

    public ICollection<RoleUserQuery> RoleUsers { get; set; }
    public ICollection<PermissionUserQuery> PermissionUsers { get; set; }
}