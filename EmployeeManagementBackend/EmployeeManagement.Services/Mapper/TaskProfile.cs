using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Services.DTO.DropDown;
using EmployeeManagement.Services.DTO.Task;
using Microsoft.CodeAnalysis;

namespace EmployeeManagement.Services.Mapper;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        // trim spaces from all data
        CreateMap<string, string>().ConvertUsing((src, dest) => src?.Trim() ?? string.Empty);

        // Add Edit Project
        CreateMap<AddEditTaskDTO, ProjectTask>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

        CreateMap<ProjectTask, TaskDetailDTO>()
           .ForMember(dest => dest.StatusName,
               opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.ProjectName, 
                opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.ReportedByName,
                opt => opt.MapFrom(src => src.ReportedByNavigation.FirstName + " " + src.ReportedByNavigation.LastName))
            .ForMember(dest => dest.AssignedToName,
                opt => opt.MapFrom(src => src.AssignedToNavigation.FirstName + " " + src.AssignedToNavigation.LastName));
    }
}
