using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class ProjectTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Code { get; set; }

    public string? Description { get; set; }

    public string Priority { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal TotalHours { get; set; }

    public string? Label { get; set; }

    public int StatusId { get; set; }

    public int ProjectId { get; set; }

    public int ReportedBy { get; set; }

    public int AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee AssignedToNavigation { get; set; } = null!;

    public virtual Employee? ModifiedByNavigation { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Employee ReportedByNavigation { get; set; } = null!;

    public virtual ProjectTaskStatus Status { get; set; } = null!;

    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}
