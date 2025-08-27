using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public string? AttendanceType { get; set; }

    public bool? IsSubmitted { get; set; }

    public string? NameOfDay { get; set; }

    public bool? IsWeekOff { get; set; }

    public bool? IsEditable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ModifyBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee? ModifyByNavigation { get; set; }

    public virtual Employee User { get; set; } = null!;
}
