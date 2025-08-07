namespace EmployeeManagement.Services.DTO.Project;

public class ProjectDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public string? Type { get; set; }
    public int? TechnologyId { get; set; }
    public string? TechnologyName { get; set; }
    public string? ProjectStatus { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EstimatedDueDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<int> AssignedEmployeeIds { get; set; } = new();
}
