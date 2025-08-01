using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string PermissionName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
