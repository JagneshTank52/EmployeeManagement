using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Services.DTO.Comment;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        CommentDetailsDTO comment = await _commentService.GetCommentByIdAsync(id);
        if (comment == null) throw new DataNotFoundException($"Comment with ID {id} not found");

        return Ok(SuccessResponse<CommentDetailsDTO>.Create(
            data: comment,
            message: Messages.Success.General.GetSuccess("Comment")
        ));
    }

    [HttpGet]
    public async Task<IActionResult> GetCommentsByTaskId([FromQuery] CommentQueryParamater parameters)
    {
        PaginatedList<CommentDetailsDTO> comments = await _commentService.GetCommentsByTaskIdAsync(parameters);
        return Ok(SuccessResponse<PaginatedList<CommentDetailsDTO>>.Create(
            data: comments,
            message: Messages.Success.General.GetSuccess("Comments")
        ));
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] AddEditCommentDTO dto)
    {
        CommentDetailsDTO created = await _commentService.AddCommentAsync(dto);
        return CreatedAtAction(nameof(GetCommentById), new { id = created.Id },
            SuccessResponse<CommentDetailsDTO>.Create(
                data: created,
                message: "Comment created successfully",
                statusCode: 201
            ));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] AddEditCommentDTO dto)
    {
        if (dto.Id != id)
            throw new DataValidationException("Id", "ID mismatch");

        CommentDetailsDTO updated = await _commentService.EditCommentAsync(dto);
        if (updated == null) throw new DataNotFoundException($"Comment {id} not found");

        return Ok(SuccessResponse<CommentDetailsDTO>.Create(
            data: updated,
            message: "Comment updated successfully"
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _commentService.DeleteCommentAsync(id);
        return Ok(SuccessResponse<bool>.Create(
            data: true,
            message: "Comment deleted successfully"
        ));
    }
}
