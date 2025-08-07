using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Technology
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
