using System.Security.Claims;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Repositories.Helper.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFActory;
    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFActory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? roleName = context.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(roleName))
        {
            throw new ForbiddenAccessException(Messages.Error.Auth.MissingClaimMessage("Role"));
        }

        using IServiceScope scope = _serviceScopeFActory.CreateAsyncScope();

        IGenericRepository<RolePermission> rolePermissionRepository = scope.ServiceProvider
            .GetRequiredService<IGenericRepository<RolePermission>>();

        var allRolePermissions = await rolePermissionRepository.GetAllAsync(
            filter: f => !f.IsDeleted,
            orderBy: o => o.Id,
            include: i => i.Include(rp => rp.Role)
                           .Include(rp => rp.Permission)
        );

        var roleWisePermission = allRolePermissions.FirstOrDefault(f =>
             f.Role.RoleName == roleName &&
             f.Permission.PermissionName == requirement.Permission
        );

        if (roleWisePermission == null)
        {
            throw new ForbiddenAccessException(Messages.Error.General.PermissionNotAssignedMessage);
        }

        bool hasPermission = requirement.PermissionType.ToLower() switch
        {
            "read" => roleWisePermission.CanRead,
            "write" => roleWisePermission.CanWrite,
            "delete" => roleWisePermission.CanDelete,
            _ => false
        };

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else
        {
            throw new ForbiddenAccessException(Messages.Error.General.ForbiddenPermissionMessage(requirement.PermissionType,requirement.Permission));
        }
    }
}
