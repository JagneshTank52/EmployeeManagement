using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Services.DTO.Project;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Retrieves a paginated list of all projects based on the provided query parameters.
    /// </summary>
    /// <param name="parameters">The filtering, sorting, and pagination parameters.</param>
    /// <returns>An ActionResult containing a paginated list of ProjectDetailDTO objects.</returns>
    [HttpGet]
    // [HasPermission(Enums.Permission.Employee, Enums.PermissionType.Read)]
    public async Task<IActionResult> GetProjectList([FromQuery] ProjectQueryParamater parameters)
    {
        var projects = await _projectService.GetProjects(parameters);

        return Ok(
            SuccessResponse<PaginatedList<ProjectDetailDTO>>.Create(
                data: projects,
                message: Messages.Success.General.GetSuccess("Projects")
            )
        );
    }

    /// <summary>
    /// Retrieves a specific project by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the project to retrieve.</param>
    /// <returns>An ActionResult containing the ProjectDetailDTO object.</returns>
    [HttpGet("{id}")]
    // [HasPermission(Enums.Permission.Project, Enums.PermissionType.Read)]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectById(id);
        return Ok(SuccessResponse<ProjectDetailDTO>.Create(project, "Project retrieved successfully"));
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="newProject">The project details to create.</param>
    /// <returns>An ActionResult containing the created ProjectDetailDTO object.</returns>
    [HttpPost]
    // [HasPermission(Enums.Permission.Project, Enums.PermissionType.Write)]
    public async Task<IActionResult> AddProject([FromBody] AddEditProjectDTO newProject)
    {
        ProjectDetailDTO? createdProjectDetails = await _projectService.AddProjectAsync(newProject);
        return CreatedAtAction(
           nameof(GetProjectById),
           new { id = createdProjectDetails!.Id },
           SuccessResponse<ProjectDetailDTO>.Create(
               data: createdProjectDetails,
               message: "project created successfully",
               statusCode: 201
           )
       );
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="id">The ID of the project to update.</param>
    /// <param name="editedProject">The updated project data.</param>
    /// <returns>An ActionResult containing the updated ProjectDetailDTO object.</returns>
    [HttpPut("{id}")]
    // [HasPermission(Enums.Permission.Employee, Enums.PermissionType.Write)]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] AddEditProjectDTO editedProject)
    {
        if (id != editedProject.Id)
        {
            throw new DataValidationException("Id", "ID in route does not match ID in request body");
        }

        ProjectDetailDTO? editedProjectDetails = await _projectService.EditProject(editedProject);

        if (editedProjectDetails == null)
        {
            throw new DataNotFoundException($"Employee with ID {id} not found or could not be updated");
        }

        return Ok
            (SuccessResponse<ProjectDetailDTO>.Create(
            data: editedProjectDetails,
            message: "Project updated successfully"));
    }

    /// <summary>
    /// Deletes a specific project by its ID.
    /// </summary>
    /// <param name="id">The ID of the project to delete.</param>
    /// <returns>An ActionResult indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}")]
    // [HasPermission(Enums.Permission.Project, Enums.PermissionType.Delete)]
    public async Task<IActionResult> DeleteProject(int id)
    {
        ProjectDetailDTO? project = await _projectService.GetProjectById(id);

        if (project == null)
        {
            throw new DataNotFoundException($"Project with ID {id} not found");
        }

        await _projectService.DeleteProject(id);

        return Ok(SuccessResponse<bool>.Create(
            data: true,
            message: "Project deleted successfully"
        ));
    }
}
