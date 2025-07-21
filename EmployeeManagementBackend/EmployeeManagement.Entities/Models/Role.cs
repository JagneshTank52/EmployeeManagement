using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
