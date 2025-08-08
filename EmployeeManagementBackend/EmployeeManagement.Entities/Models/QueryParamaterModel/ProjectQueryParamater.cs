namespace EmployeeManagement.Entities.Models.QueryParamaterModel;

public class ProjectQueryParamater : PaginationQueryParamater
{
    public int? TechnologyId { get; set; }
    public string? ProjectStatus { get; set; }
    public string? Type { get; set; }
}
