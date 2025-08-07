using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class ProjectEmployee
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime AssignedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee? ModifiedByNavigation { get; set; }

    public virtual Project Project { get; set; } = null!;
}
