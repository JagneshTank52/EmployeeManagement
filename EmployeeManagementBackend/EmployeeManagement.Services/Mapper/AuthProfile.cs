using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO.Auth;

namespace EmployeeManagement.Services.Mapper;

public class AuthProfile : Profile, IAutoMapper
{
    public AuthProfile()
    {
        // Register request
        CreateMap<RegisterRequestDTO, Employee>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)))
            .ForMember(dest => dest.HashPassword, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
