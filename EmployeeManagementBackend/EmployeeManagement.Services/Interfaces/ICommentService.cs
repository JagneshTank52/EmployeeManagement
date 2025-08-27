using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Services.DTO.Comment;

namespace EmployeeManagement.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDetailsDTO> GetCommentByIdAsync(int id);
    Task<PaginatedList<CommentDetailsDTO>> GetCommentsByTaskIdAsync(CommentQueryParamater parameters);
    Task<CommentDetailsDTO> AddCommentAsync(AddEditCommentDTO dto);
    Task<CommentDetailsDTO> EditCommentAsync(AddEditCommentDTO dto);
    Task DeleteCommentAsync(int id);
}
