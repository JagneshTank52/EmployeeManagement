using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.Attendance;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementations;

public class AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper) : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository = attendanceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<AttendanceSheetDTO> GetAttendanceSheetAsync(int userId, int month, int year)
    {
        List<Attendance> userAttendanceRecord = (await _attendanceRepository.GetAllAsync(
            filter: f => !f.IsDeleted && f.UserId == userId && f.AttendanceDate.Month == month && f.AttendanceDate.Year == year,
            include: i => i.Include(a => a.User)
        )).ToList();

        // Month range
        DateTime monthStart = new DateTime(year, month, 1);
        DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

        List<AttendanceDetailsDTO> attendanceList = Enumerable.Range(0, monthEnd.Day)
            .Select(offset =>
            {
                DateTime date = monthStart.AddDays(offset);
                Attendance? existingRecord = userAttendanceRecord.FirstOrDefault(a => a.AttendanceDate.Date == date.Date);

                return new AttendanceDetailsDTO
                {
                    AttendanceDate = date,
                    AttendanceType =  existingRecord != null ? existingRecord.AttendanceType : null,
                    IsSubmitted = existingRecord != null,
                    NameOfDay = date.DayOfWeek.ToString(),
                    IsWeekOff = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday,
                    IsEditable = true
                };
            })
            .ToList();

        AttendanceSheetDTO response = new AttendanceSheetDTO
        {
            JoiningDate = userAttendanceRecord.FirstOrDefault()!.User.CreatedAt,
            Attendance = attendanceList,
            PresentDay = userAttendanceRecord.Count(c => c.AttendanceType == "P"),
            AbsentDay = userAttendanceRecord.Count(c => c.AttendanceType == "A")
        };

        return response;
    }

    public async Task<AttendanceDetailsDTO> SubmitAttendanceAsync(int userId, AttendanceDetailsDTO attendance)
    {
        if (attendance == null)
             throw new ArgumentNullException(nameof(attendance));

        Attendance? existingRecord = await _attendanceRepository.GetFirstOrDefaultAsync(
            filter: a => a.UserId == userId && a.AttendanceDate.Date == attendance.AttendanceDate.Date
        );

        Attendance entity;

        if (existingRecord == null)
        {

            Attendance newAttendance = _mapper.Map<Attendance>(attendance);
            newAttendance.UserId = 3007;

            entity = await _attendanceRepository.AddEntityAsync(newAttendance);
        }
        else
        {
            _mapper.Map(attendance,existingRecord);
             entity = await _attendanceRepository.UpdateAsync(existingRecord);
        }

        
        return _mapper.Map<AttendanceDetailsDTO>(entity);
    }

}
