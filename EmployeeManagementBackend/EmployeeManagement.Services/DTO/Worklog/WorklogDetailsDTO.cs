namespace EmployeeManagement.Services.DTO.Worklog;

public class WorklogDetailsDTO
{
    public int Id { get; set; }
    public string WorkLogTitle { get; set; } = null!;
    public string Code { get; set; } = null!; 
    public int TaskId { get; set; }
    public string TaskCode { get; set; } = null!; //
    public string TaskTitle { get; set; } = null!; //
    public string AssignedToName { get; set; } = null!; //
    public DateTime WorkDate { get; set; } 
    public string TaskStatusName { get; set; } = null!; //
    public string? TaskStatusColor { get; set; } //
    public decimal WorkTimeHours { get; set; }

    public bool IsEditable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
