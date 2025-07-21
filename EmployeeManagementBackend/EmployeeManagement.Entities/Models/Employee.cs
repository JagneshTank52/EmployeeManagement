using System;
using System.Collections.Generic;

namespace EmployeeManagement.Entities.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public int RoleId { get; set; }

    public string? PhoneNumber { get; set; }

    public string Gender { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? Address { get; set; }

    public int DepartmentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
