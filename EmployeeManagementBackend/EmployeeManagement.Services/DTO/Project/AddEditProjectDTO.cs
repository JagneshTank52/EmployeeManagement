namespace EmployeeManagement.Services.DTO.Project;

public class AddEditProjectDTO
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required int TechnologyId { get; set; }
    public required string ProjectStatus { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EstimatedDueDate { get; set; }
    public decimal EstimatedHours { get; set; }

    // List of assigned Employee IDs
    public List<int> AssignedEmployeeIds { get; set; } = new();
}
