namespace EmployeeManagement.Services.DTO.Worklog;

public class WorkSheetTaskDetailsDTO
{
    public int Id {get; set;}

    public string Code {get; set;}

    public string Title {get; set;}
    
    public int TotalTimeSpent {get; set;}

    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}

    public int TaskStatus {get; set;}

    public string StatusName {get; set;}

    public string StatusColor {get; set;}

    public List<WorkSheetWorklogDTO> workLogDetails {get; set;}
}
