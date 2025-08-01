
using EmployeeManagement.Entities.Shared.Constant;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Repositories.Helper.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Enums.Permission permission, Enums.PermissionType permissionType)
              : base(policy: $"{permission}_{permissionType}") { }
}
