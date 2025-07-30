using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.DTO.Auth;

namespace EmployeeManagement.Services.Helpers;

public class MappingPeofile : Profile
{
    public MappingPeofile()
    {
        // global mapping use full to conver one data type to other
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
             .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
             .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

        CreateMap<EmployeeDetailDTO, Employee>()
             .ForMember(dest => dest.Department, opt => opt.Ignore())
             .ForMember(dest => dest.Role, opt => opt.Ignore());

        // Register request
        CreateMap<RegisterRequestDTO, Employee>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)))
            .ForMember(dest => dest.HashPassword, opt => opt.MapFrom(src => src.Password)) // hash later in service
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); 
    }
}
