namespace EmployeeManagement.Entities.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; private set; }
    public int PageIndex { get; private set;}
    public int PageSize { get; private set;}
    public int TotalPages { get; private set;}
    public int TotalCounts { get; private set;}
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int pageIndex, int pageSize, int totalCounts)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalCounts = totalCounts;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCounts / (double)pageSize);
    }
}
