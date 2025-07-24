using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Services.DTO;

public class AddEmployeeDTO
{
    public int Id {get; set;}
    
    // Employee Details
    [Required(ErrorMessage = "First Name is required.")]
    [MaxLength(255)]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "last Name is required.")]
    [MaxLength(255)]
    public required string LastName { get; set; }

    [MaxLength(20)]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [MaxLength(20)]
    public required string Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is required.")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    [MaxLength(255)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department ID is required.")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "User Name is required for the user account.")]
    [MaxLength(255)]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Email is required for the user account.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; } 

    [Required(ErrorMessage = "Role ID is required for the user account.")]
    public int RoleId { get; set; }
}
