

using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Services.DTO.Attendance;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController(IAttendanceService attendanceService) : ControllerBase
{
    private readonly IAttendanceService _attendanceService = attendanceService;

    [HttpGet]
    public async Task<IActionResult> GetAttendance([FromQuery] int userId, [FromQuery] int month, [FromQuery] int year)
    {
        var attendances = await _attendanceService.GetAttendanceSheetAsync(userId, month, year);

        return Ok(SuccessResponse<AttendanceSheetDTO>.Create(
            data: attendances,
            message: Messages.Success.General.GetSuccess("Attendance Sheet")
        ));
    }


    [HttpPost("submit/{userId}")]
    public async Task<IActionResult> SubmitAttendance(int userId, [FromBody] AttendanceDetailsDTO attendance)
    {
        AttendanceDetailsDTO saved = await _attendanceService.SubmitAttendanceAsync(userId, attendance);

        return CreatedAtAction(nameof(GetAttendance),
            new { userId = userId, month = saved.AttendanceDate.Month, year = saved.AttendanceDate.Year },
            SuccessResponse<AttendanceDetailsDTO>.Create(
                data: saved,
                message: Messages.Success.General.PostSuccess("Attendance"),
                statusCode: StatusCodes.Status201Created
            ));
    }
}
