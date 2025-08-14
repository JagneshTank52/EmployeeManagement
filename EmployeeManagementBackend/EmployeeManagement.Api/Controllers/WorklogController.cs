
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Services.DTO.Task;
using EmployeeManagement.Services.DTO.Worklog;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorklogController : ControllerBase
{
    private readonly IWorklogService _worklogService;
    public WorklogController(IWorklogService service) => _worklogService = service;

    [HttpGet]
    public async Task<IActionResult> GetWorkLogs([FromQuery] WorklogQueryParamater parameters)
    {
        var workLogs = await _worklogService.GetWorkLogsAsync(parameters);
        return Ok(SuccessResponse<PaginatedList<WorklogDetailsDTO>>.Create(
            data: workLogs,
            message: Messages.Success.General.GetSuccess("Work logs")
        ));
    }

    [HttpGet("work-sheet")]
    public async Task<IActionResult> GetWorkSheet([FromQuery] int month, [FromQuery] int year)
    {
        WorkSheetDetailsDTO workSheet = await _worklogService.GetWorkSheetAsync(month, year);

        return Ok(SuccessResponse<WorkSheetDetailsDTO>.Create(
    data: workSheet,
    message: Messages.Success.General.GetSuccess("Work logs")
));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkLogById(int id)
    {
        var worklog = await _worklogService.GetWorkLogByIdAsync(id);
        if (worklog == null) throw new DataNotFoundException($"WorkLog with ID {id} not found");
        return Ok(SuccessResponse<WorklogDetailsDTO>.Create(
            data: worklog,
            message: Messages.Success.General.GetSuccess("Work log")
        ));
    }

    [HttpPost]
    public async Task<IActionResult> AddWorkLog([FromBody] AddEditWorklogDTO dto)
    {
        var created = await _worklogService.AddWorkLogAsync(dto);
        return CreatedAtAction(nameof(GetWorkLogById), new { id = created.Id },
            SuccessResponse<WorklogDetailsDTO>.Create(
                data: created,
                message: "WorkLog created successfully",
                statusCode: 201
            ));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkLog(int id, [FromBody] AddEditWorklogDTO dto)
    {
        if (dto.Id != id)
            throw new DataValidationException("Id", "ID in route does not match ID in request body");

        var updated = await _worklogService.EditWorkLogAsync(dto);

        if (updated == null)
            throw new DataNotFoundException($"WorkLog with ID {id} not found or could not be updated");

        return Ok(SuccessResponse<WorklogDetailsDTO>.Create(
            data: updated,
            message: "Worklog updated successfully"
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkLog(int id)
    {
        var existing = await _worklogService.GetWorkLogByIdAsync(id);

        if (existing == null) throw new DataNotFoundException($"WorkLog with ID {id} not found");

        await _worklogService.DeleteWorkLogAsync(id);

        return Ok(SuccessResponse<bool>.Create(
            data: true,
            message: "WorkLog deleted successfully"
        ));
    }
}
