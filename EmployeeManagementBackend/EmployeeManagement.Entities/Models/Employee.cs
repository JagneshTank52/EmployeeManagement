using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? UserName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? PhoneNumber { get; set; }

    public string Gender { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? Address { get; set; }

    public int? DepartmentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Attendance> AttendanceModifyByNavigations { get; set; } = new List<Attendance>();

    public virtual ICollection<Attendance> AttendanceUsers { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<ProjectEmployee> ProjectEmployeeEmployees { get; set; } = new List<ProjectEmployee>();

    public virtual ICollection<ProjectEmployee> ProjectEmployeeModifiedByNavigations { get; set; } = new List<ProjectEmployee>();

    public virtual ICollection<ProjectTask> ProjectTaskAssignedToNavigations { get; set; } = new List<ProjectTask>();

    public virtual ICollection<ProjectTask> ProjectTaskModifiedByNavigations { get; set; } = new List<ProjectTask>();

    public virtual ICollection<ProjectTask> ProjectTaskReportedByNavigations { get; set; } = new List<ProjectTask>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}
