using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO.Project;
namespace EmployeeManagement.Services.Mapper;

public class ProjectProfile : Profile, IAutoMapper
{
    public ProjectProfile()
    {
        // trim spaces from all data
        CreateMap<string, string>().ConvertUsing((src, dest) => src?.Trim() ?? string.Empty);

        // Add Edit Project
        CreateMap<AddEditProjectDTO, Project>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

        CreateMap<Project, ProjectDetailDTO>()
           .ForMember(dest => dest.TechnologyName,
               opt => opt.MapFrom(src => src.Technology!.Name))
           .ForMember(dest => dest.AssignedEmployee,
               opt => opt.MapFrom(
                   src => src.ProjectEmployees.Select(pe => new AssignedEmployeeDTO
                   {
                       Id = pe.EmployeeId,
                       UserName = pe.Employee != null ? pe.Employee.UserName! : string.Empty
                   }).ToList()
               ));
    }
}
