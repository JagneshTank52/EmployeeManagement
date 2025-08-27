namespace EmployeeManagement.Services.DTO.Attendance;

public class AttendanceDetailsDTO
{
    public DateTime AttendanceDate { get; set; }
    public string? AttendanceType { get; set; }
    public bool IsSubmitted { get; set; }
    public string? NameOfDay { get; set; }
    public bool IsWeekOff { get; set; }
    public bool IsEditable { get; set; }
}
