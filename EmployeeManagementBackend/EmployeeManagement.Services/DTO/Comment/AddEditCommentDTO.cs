namespace EmployeeManagement.Services.DTO.Comment;

public class AddEditCommentDTO
{
    public int? Id { get; set; }
    public int TaskId { get; set; }
    public string Comment { get; set; } = null!;
}
