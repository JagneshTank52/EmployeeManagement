

namespace EmployeeManagement.Services.DTO.Worklog;
public class DailyWorklogDetailsDTO
{
    public DateOnly AttendanceDate { get; set; }

    public string Day { get; set; }

    public int? WeeklyTotalTimeInMinutes { get; set; }

    public int DailyWorkLoginMinutes { get; set; }
}