using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public string? Type { get; set; }

    public int? TechnologyId { get; set; }

    public string? ProjectStatus { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EstimatedDueDate { get; set; }

    public decimal? EstimatedHours { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee? ModifiedByNavigation { get; set; }

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();

    public virtual Technology? Technology { get; set; }
}
