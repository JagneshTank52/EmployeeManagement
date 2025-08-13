namespace EmployeeManagement.Services.DTO.Task;

public class TaskDetailDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; } = "Low";
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty; //
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalHours { get; set; }
    public string? Label { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty; //
    public int ReportedBy { get; set; }
    public string ReportedByName { get; set; } = string.Empty; //
    public int AssignedTo { get; set; }
    public string AssignedToName { get; set; } = string.Empty; //
    public DateTime CreatedAt { get; set; }
}
