namespace EmployeeManagement.Entities.Models.QueryParamaterModel;

public class TaskQueryParameter : PaginationQueryParamater
{
    public int? StatusId { get; set; }
    public string? Priority { get; set; }
    public int? ProjectId { get; set; }
    public int? AssignedTo { get; set; }
}
