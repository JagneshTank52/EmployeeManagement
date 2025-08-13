using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Services.DTO.Task;

namespace EmployeeManagement.Services.Interfaces;

public interface ITaskService
{
    Task<PaginatedList<TaskDetailDTO>> GetTasksAsync(TaskQueryParameter parameters);
    Task<TaskDetailDTO> GetTaskByIdAsync(int id);
    Task<TaskDetailDTO> AddTaskAsync(AddEditTaskDTO dto);
    Task<TaskDetailDTO> EditTaskAsync(AddEditTaskDTO dto);
    Task DeleteTaskAsync(int id);
}
