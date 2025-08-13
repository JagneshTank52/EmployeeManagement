using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Services.DTO.Task;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TaskController(ITaskService taskService) => _taskService = taskService;

    [HttpGet]
    // [HasPermission(Enums.Permission.Employee, Enums.PermissionType.Read)]
    public async Task<IActionResult> GetProjectList([FromQuery] TaskQueryParameter parameters)
    {
        var tasks = await _taskService.GetTasksAsync(parameters);

        return Ok(
            SuccessResponse<PaginatedList<TaskDetailDTO>>.Create(
                data: tasks,
                message: Messages.Success.General.GetSuccess("Tasks")
            )
        );
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        return Ok(SuccessResponse<TaskDetailDTO>.Create(task, "Task retrieved successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] AddEditTaskDTO dto)
    {
        TaskDetailDTO? createdTask = await _taskService.AddTaskAsync(dto);

        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id },
            SuccessResponse<TaskDetailDTO>.Create(createdTask, "Task created successfully", 201));
    }

    [HttpPut("{id}")]
    // [HasPermission(Enums.Permission.Employee, Enums.PermissionType.Write)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] AddEditTaskDTO dto)
    {
        if (id != dto.Id)
        {
            throw new DataValidationException("Id", "ID in route does not match ID in request body");
        }

        TaskDetailDTO? taskDetail = await _taskService.EditTaskAsync(dto);

        if (taskDetail == null)
        {
            throw new DataNotFoundException($"Task with ID {id} not found or could not be updated");
        }

        return Ok
            (SuccessResponse<TaskDetailDTO>.Create(
            data: taskDetail,
            message: "Task updated successfully"));
    }

    [HttpDelete("{id}")]
    // [HasPermission(Enums.Permission.Project, Enums.PermissionType.Delete)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        TaskDetailDTO? task = await _taskService.GetTaskByIdAsync(id);

        if (task == null)
        {
            throw new DataNotFoundException($"Task with ID {id} not found");
        }

        await _taskService.DeleteTaskAsync(id);

        return Ok(SuccessResponse<bool>.Create(
            data: true,
            message: "Task deleted successfully"
        ));
    }
}
