using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Services.DTO;

public class AddEmployeeDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name Reqired")]
    [StringLength(100, ErrorMessage = "First Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "First Name Reqired")]
    [StringLength(100, ErrorMessage = "First Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid phone number")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
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

    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
    ErrorMessage = "Password must contain upper, lower, digit, and special character")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Role ID is required for the user account.")]
    public int RoleId { get; set; }
}
