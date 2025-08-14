namespace EmployeeManagement.Services.DTO.Worklog;

public class WorkSheetWorklogDTO
{
    public int Id {get; set;}

    public DateTime AttendanceDate {get; set;}

    public string Day {get; set;}

    public bool IsEnable {get; set;}

    public bool IsHoliday {get; set;}

    public int WorkTimeInMinute {get; set;}


}
