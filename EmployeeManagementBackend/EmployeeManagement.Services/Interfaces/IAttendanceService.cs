using EmployeeManagement.Services.DTO.Attendance;

namespace EmployeeManagement.Services.Interfaces;

public interface IAttendanceService 
{
    Task<AttendanceSheetDTO> GetAttendanceSheetAsync(int userId, int month, int year);

    Task<AttendanceDetailsDTO> SubmitAttendanceAsync(int userId, AttendanceDetailsDTO attendance);
}
