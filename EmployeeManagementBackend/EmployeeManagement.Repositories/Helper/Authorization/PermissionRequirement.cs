using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Repositories.Helper.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission, string permissionType)
    {
        Permission = permission;
        PermissionType = permissionType;
    }
    public string Permission {get; }
    public string PermissionType {get;}
}
