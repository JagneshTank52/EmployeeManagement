using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Services.DTO;

public class EmployeeDetailDTO
{
    // Employee Details
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName {get; set;} = string.Empty;
    public string Email { get; set; }= string.Empty;
    public string Password { get; set; } = string.Empty;
     public int RoleId { get; set; }
    public string RoleName {get; set;} = string.Empty;
}
