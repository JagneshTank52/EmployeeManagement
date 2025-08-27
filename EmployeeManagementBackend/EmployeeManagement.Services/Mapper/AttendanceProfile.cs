using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO.Attendance;

namespace EmployeeManagement.Services.Mapper;

public class AttendanceProfile : Profile
{
    public AttendanceProfile()
    {
        CreateMap<AttendanceDetailsDTO, Attendance>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));
        
        CreateMap<Attendance,AttendanceDetailsDTO>();
    }
}
