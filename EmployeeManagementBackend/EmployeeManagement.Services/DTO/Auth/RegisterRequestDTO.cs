using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Services.DTO.Auth;

public class RegisterRequestDTO
{
    [Required(ErrorMessage = "First Name Reqired")]
    [StringLength(100, ErrorMessage = "First Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "First Name Reqired")]
    [StringLength(100, ErrorMessage = "First Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    // [CustomValidation(typeof(RegisterRequestDTO), nameof(ValidateAge))]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
    public required string Gender { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid phone number")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
    ErrorMessage = "Password must contain upper, lower, digit, and special character")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public required string ConfirmPassword { get; set; }
}

  // Custom age validation method
    // public static ValidationResult? ValidateAge(DateTime dob, ValidationContext context)
    // {
    //     var age = DateTime.Today.Year - dob.Year;
    //     if (dob > DateTime.Today.AddYears(-age)) age--;

    //     return age >= 13
    //         ? ValidationResult.Success
    //         : new ValidationResult("User must be at least 13 years old");
    // }