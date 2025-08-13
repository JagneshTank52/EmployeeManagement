namespace EmployeeManagement.Services.DTO.Project;

public class ProjectDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public string? Type { get; set; }
    public int? TechnologyId { get; set; }
    public string TechnologyName { get; set; } = string.Empty;
    public string? ProjectStatus { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EstimatedDueDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AssignedEmployeeDTO> AssignedEmployee {get; set;}  = new ();
}
