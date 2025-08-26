using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class WorkLog
{
    public int Id { get; set; }

    public string WorkLogTitle { get; set; } = null!;

    public string? Code { get; set; }

    public int TaskId { get; set; }

    public DateTime WorkDate { get; set; }

    public decimal WorkTimeInMinutes { get; set; }

    public bool? IsEditable { get; set; }

    public string? Description { get; set; }

    public int WorkDoneBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ProjectTask Task { get; set; } = null!;

    public virtual Employee WorkDoneByNavigation { get; set; } = null!;
}
