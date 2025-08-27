using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.DTO.Comment;

namespace EmployeeManagement.Services.Mapper;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<TaskComment, CommentDetailsDTO>()
            .ForMember(dest => dest.TaskCode,
                opt => opt.MapFrom(src => src.Task.Code)) // assuming Task has Code
            .ForMember(dest => dest.TaskTitle,
                opt => opt.MapFrom(src => src.Task.Title)) // assuming Task has Title
            .ForMember(dest => dest.Comment,
                opt => opt.MapFrom(src => src.Comment)) // mapping Comment1 → Comment
            .ForMember(dest => dest.CommentByName,
                opt => opt.MapFrom(src => src.CommentByNavigation.FirstName + " " + src.CommentByNavigation.LastName));

        CreateMap<TaskComment,AddEditCommentDTO>().ReverseMap();
    }
}
