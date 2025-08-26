namespace EmployeeManagement.Services.DTO.Worklog;

public class AddEditWorklogDTO
{
    public int? Id { get; set; }
    public string WorkLogTitle { get; set; } = null!;
    public int TaskId { get; set; }
    public DateTime WorkDate { get; set; }
    public string Description {get;set;} = string.Empty;
    public decimal WorkTimeInMinutes { get; set; }
}
