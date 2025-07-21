using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO;

namespace EmployeeManagement.Services.Helpers;

public class MappingPeofile : Profile
{
    public MappingPeofile()
    {
        // global mapping use full to conver one data type to other
        CreateMap<string, string>().ConvertUsing((src, dest) => src.Trim());

        // Add Employee DTO
        CreateMap<AddEmployeeDTO, Employee>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ReverseMap();
            // .ForMember(dest => dest.HashPassword, opt => opt.Ignore()) 
            // .ForMember(dest => dest.Password, opt => opt.Ignore()) 
            // .ForMember(dest => dest.Role, opt => opt.Ignore());

        // Employee Details
       CreateMap<Employee, EmployeeDetailDTO>()           
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name)) 
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

       CreateMap<EmployeeDetailDTO, Employee>()           
            .ForMember(dest => dest.Department, opt => opt.Ignore()) 
            .ForMember(dest => dest.Role, opt => opt.Ignore());


    }
}
