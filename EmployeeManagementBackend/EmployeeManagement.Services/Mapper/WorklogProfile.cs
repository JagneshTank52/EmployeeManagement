using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO.Worklog;

namespace EmployeeManagement.Services.Mapper;

public class WorklogProfile : Profile
{
    public WorklogProfile()
    {
        CreateMap<AddEditWorklogDTO, WorkLog>()
            .ForMember(dest => dest.WorkDoneBy, opt => opt.MapFrom(_ => 3007))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));


        CreateMap<WorkLog, WorklogDetailsDTO>()
            .ForMember(d => d.TaskCode, o => o.MapFrom(s => s.Task.Code))
            .ForMember(d => d.TaskTitle, o => o.MapFrom(s => s.Task.Title))
            .ForMember(d => d.AssignedToName, o => o.MapFrom(s => s.Task.AssignedToNavigation.FirstName + " " + s.Task.AssignedToNavigation.LastName))
            .ForMember(d => d.TaskStatusName, o => o.MapFrom(s => s.Task.Status.Name))
            .ForMember(d => d.TaskStatusColor, o => o.MapFrom(s => s.Task.Status.Color))
            .ForMember(d => d.IsEditable, o => o.MapFrom(s => (DateTime.UtcNow.Date - s.WorkDate.Date).TotalDays <= 3));

        CreateMap<Project, WorkSheetDetailsDTO>()
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProjectCode, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ProjectTotalMinutes,
                opt => opt.MapFrom(src =>
                    src.ProjectTasks
                        .SelectMany(t => t.WorkLogs)
                        .Where(wl => !wl.IsDeleted)
                        .Sum(wl => wl.WorkTimeHours * 60)
                ))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.ProjectTasks));

        CreateMap<ProjectTask, WorkSheetTaskDetailsDTO>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
          .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
          .ForMember(dest => dest.TotalTimeSpent,
              opt => opt.MapFrom(src =>
                  src.WorkLogs.Where(wl => !wl.IsDeleted)
                              .Sum(wl => wl.WorkTimeHours * 60)
              ))
          .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
          .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate ?? DateTime.MinValue))
          .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.Status.Id))
          .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
          .ForMember(dest => dest.StatusColor, opt => opt.MapFrom(src => src.Status.Color))
          .ForMember(dest => dest.workLogDetails, opt => opt.MapFrom(src => src.WorkLogs));

          CreateMap<WorkLog, WorkSheetWorklogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AttendanceDate, opt => opt.MapFrom(src => src.WorkDate))
            .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.WorkDate.DayOfWeek.ToString()))
            .ForMember(dest => dest.IsEnable, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsHoliday, opt => opt.MapFrom(src => src.WorkDate.DayOfWeek == DayOfWeek.Sunday))
            .ForMember(dest => dest.WorkTimeInMinute, opt => opt.MapFrom(src => (int)(src.WorkTimeHours * 60)));


    }
}
