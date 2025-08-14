namespace EmployeeManagement.Services.DTO.Worklog;

public class WorkSheetDetailsDTO
{
    public int ProjectId {get; set;}

    public string ProjectCode {get; set;}

    public string ProjectName {get; set;}

    public decimal ProjectTotalMinutes {get; set;}

    public List<WorkSheetTaskDetailsDTO> Tasks {get; set;}

}
