using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class RolePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public bool CanRead { get; set; }

    public bool CanWrite { get; set; }

    public bool CanDelete { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
