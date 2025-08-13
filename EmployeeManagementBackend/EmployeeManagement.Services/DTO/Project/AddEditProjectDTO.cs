using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Services.DTO.Project;

public class AddEditProjectDTO
{
    public int Id {get; set;}

    [Required(ErrorMessage = "Project name is required.")]
    [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Project name can only contain letters, numbers, and spaces.")]
    public required string Name { get; set; }

        [Required(ErrorMessage = "Project type is required.")]
        [StringLength(100, ErrorMessage = "Project type cannot exceed 100 characters.")]
        public required string Type { get; set; }

    [Required(ErrorMessage = "Technology selection is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "TechnologyId must be a positive number.")]
    public required int TechnologyId { get; set; }

    [Required(ErrorMessage = "Project status is required.")]
    [StringLength(100, ErrorMessage = "Project status cannot exceed 100 characters.")]
    public required string ProjectStatus { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Estimated due date is required.")]
    [DateGreaterThan(nameof(StartDate), ErrorMessage = "Estimated due date must be after the start date.")]
    public DateTime EstimatedDueDate { get; set; }

    [Required(ErrorMessage = "Estimated hours are required.")]
    [Range(1, 10000, ErrorMessage = "Estimated hours must be between 1 and 10,000.")]
    public decimal EstimatedHours { get; set; }

    public List<int> AssignedEmployeeIds { get; set; } = new();
}
