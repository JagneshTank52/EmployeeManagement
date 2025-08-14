using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Services.DTO.Worklog;

namespace EmployeeManagement.Services.Interfaces;

public interface IWorklogService
{
    Task<PaginatedList<WorklogDetailsDTO>> GetWorkLogsAsync(WorklogQueryParamater parameters);
    Task<WorkSheetDetailsDTO> GetWorkSheetAsync(int month, int year);
    Task<WorklogDetailsDTO> GetWorkLogByIdAsync(int id);
    Task<WorklogDetailsDTO> AddWorkLogAsync(AddEditWorklogDTO dto);
    Task<WorklogDetailsDTO> EditWorkLogAsync(AddEditWorklogDTO dto);
    Task DeleteWorkLogAsync(int id);
}

