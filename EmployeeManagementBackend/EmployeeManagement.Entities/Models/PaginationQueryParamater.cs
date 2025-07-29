namespace EmployeeManagement.Entities.Models;

public class PaginationQueryParamater
{
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : value;
    }
    public string? SortBy { get; set; } 
    public string? SearchTerm { get; set; }
}
