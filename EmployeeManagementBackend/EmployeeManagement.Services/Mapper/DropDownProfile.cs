using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO.DropDown;

namespace EmployeeManagement.Services.Mapper;

public class DropDownProfile : Profile
{
    public DropDownProfile()
    {
        CreateMap<ProjectTaskStatus, DropDownListDTO>();
        CreateMap<Employee, DropDownListDTO>();
        CreateMap<Technology, DropDownListDTO>();
        CreateMap<Project, DropDownListDTO>();
    }
}
