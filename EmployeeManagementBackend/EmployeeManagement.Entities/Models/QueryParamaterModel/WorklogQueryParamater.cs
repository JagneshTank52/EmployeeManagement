namespace EmployeeManagement.Entities.Models.QueryParamaterModel;

public class WorklogQueryParamater : PaginationQueryParamater
{
    public int? TaskId { get; set; }
    public int? AssignedToId { get; set; }
}
