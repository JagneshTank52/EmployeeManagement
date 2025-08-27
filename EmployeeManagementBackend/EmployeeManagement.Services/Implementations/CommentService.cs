using System.Linq.Expressions;
using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.Comment;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementations;

public class CommentService(ICommentRepository commentRepository, IMapper mapper) : ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommentDetailsDTO> GetCommentByIdAsync(int id)
    {
        TaskComment? comment = await _commentRepository.GetFirstOrDefaultAsync(
            filter: f => !f.IsDeleted && !f.Task.IsDeleted && f.Id == id,
            include: i => i.Include(a => a.Task).Include(b => b.CommentByNavigation)
            ?? throw new DataNotFoundException($"Comment with ID {id} not found")
        );

        return _mapper.Map<CommentDetailsDTO>(comment);
    }

    public async Task<PaginatedList<CommentDetailsDTO>> GetCommentsByTaskIdAsync(CommentQueryParamater parameters)
    {
        Expression<Func<TaskComment, bool>> filter = p => !p.IsDeleted;

        if (parameters.TaskId.HasValue)
            filter = filter.AndAlso(p => p.TaskId == parameters.TaskId);


        Func<IQueryable<TaskComment>, IOrderedQueryable<TaskComment>> orderBy = parameters.SortBy switch
        {
            "name_asc" => q => q.OrderBy(p => p.Id),
            "name_desc" => q => q.OrderByDescending(p => p.Id),
            _ => q => q.OrderByDescending(p => p.Id)
        };

        Func<IQueryable<TaskComment>, IQueryable<TaskComment>> include = query => query
            .Include(a => a.Task).Include(b => b.CommentByNavigation);


        var result = await _commentRepository.GetPagedRecords(
            parameters.PageSize,
            parameters.PageNumber,
            orderBy,
            filter,
            include
        );

        return new PaginatedList<CommentDetailsDTO>(
            _mapper.Map<List<CommentDetailsDTO>>(result.records),
            result.pageIndex,
            result.pageSize,
            result.totalRecord
        );
    }

    public async Task<CommentDetailsDTO> AddCommentAsync(AddEditCommentDTO dto)
    {
        TaskComment newComment = _mapper.Map<TaskComment>(dto);
        newComment.CommentBy = 3007;
        TaskComment comment = await _commentRepository.AddEntityAsync(newComment);
        return await GetCommentByIdAsync(comment.Id);
    }

    public async Task<CommentDetailsDTO> EditCommentAsync(AddEditCommentDTO dto)
    {
        if (!dto.Id.HasValue) throw new DataValidationException("Id", "Id is required for update");

        TaskComment? existingComment = await _commentRepository.GetFirstOrDefaultAsync(
            filter: f => f.Id == dto.Id && !f.IsDeleted);

        _mapper.Map(dto, existingComment);

        TaskComment? updatedComment = await _commentRepository.UpdateAsync(existingComment!);

        return await GetCommentByIdAsync(updatedComment!.Id);
    }

    public async Task DeleteCommentAsync(int id)
    {
        TaskComment? commentToDelete = await _commentRepository.GetByIdAsync(id);

        if (commentToDelete == null)
        {
            throw new DataNotFoundException($"Comment with ID {id} not found");
        }

        commentToDelete.IsDeleted = true;
        commentToDelete.UpdatedAt = DateTime.UtcNow;

        await _commentRepository.DeleteAsync(commentToDelete);
    }
}
