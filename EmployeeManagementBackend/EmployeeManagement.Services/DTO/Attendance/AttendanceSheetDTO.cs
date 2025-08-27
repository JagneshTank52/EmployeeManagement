namespace EmployeeManagement.Services.DTO.Attendance;

public class AttendanceSheetDTO
{
    public DateTime JoiningDate { get; set; }
    public List<AttendanceDetailsDTO> Attendance { get; set; } = new();
    public int PresentDay { get; set; }
    public int AbsentDay { get; set; }
}
