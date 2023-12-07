#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.RoleUser.Entities;

namespace Karami.Domain.User.Entities;

public class UserQuery : BaseEntityQuery<string>
{
    public string FirstName  { get; set; }
    public string LastName   { get; set; }
    public string Username   { get; set; }
    public string Password   { get; set; }
    public IsActive IsActive { get; set; }

    /*---------------------------------------------------------------*/
    
    //Relations

    public ICollection<RoleUserQuery> RoleUsers { get; set; }
    public ICollection<PermissionUserQuery> PermissionUsers { get; set; }
}