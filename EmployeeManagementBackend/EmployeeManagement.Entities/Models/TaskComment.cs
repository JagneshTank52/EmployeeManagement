using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class TaskComment
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string Comment { get; set; } = null!;

    public int CommentBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee CommentByNavigation { get; set; } = null!;

    public virtual ProjectTask Task { get; set; } = null!;
}
