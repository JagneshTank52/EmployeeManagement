namespace EmployeeManagement.Services.DTO.Comment;

public class CommentDetailsDTO
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string TaskCode {get; set;} = null!;
    public string TaskTitle {get; set;} = null!;
    public string Comment { get; set; } = null!;
    public int CommentBy { get; set; }
    public string CommentByName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
