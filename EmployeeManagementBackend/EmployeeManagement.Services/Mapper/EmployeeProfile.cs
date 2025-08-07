using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO;

namespace EmployeeManagement.Services.Mapper;

public class EmployeeProfile : Profile, IAutoMapper
{
    public EmployeeProfile()
    {
        // trim spaces from all data
        CreateMap<string, string>().ConvertUsing((src, dest) => src?.Trim() ?? string.Empty);

        // Add Employee DTO
        CreateMap<AddEmployeeDTO, Employee>()
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ReverseMap();
        // .ForMember(dest => dest.HashPassword, opt => opt.Ignore()) 
        // .ForMember(dest => dest.Password, opt => opt.Ignore()) 
        // .ForMember(dest => dest.Role, opt => opt.Ignore());

        // Employee Details
        CreateMap<Employee, EmployeeDetailDTO>()
             .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department!.Name))
             .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role!.RoleName));

        CreateMap<EmployeeDetailDTO, Employee>()
             .ForMember(dest => dest.Department, opt => opt.Ignore())
             .ForMember(dest => dest.Role, opt => opt.Ignore());

    }
}
