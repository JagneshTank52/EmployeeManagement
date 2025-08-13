namespace EmployeeManagement.Services.DTO.Task;

public class AddEditTaskDTO
{
    public int Id { get; set; } // For edit
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = "Low";
    public int StatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalHours { get; set; }
    public string? Label { get; set; }
    public int ProjectId { get; set; }
    public int ReportedBy { get; set; }
    public int AssignedTo { get; set; }
}
