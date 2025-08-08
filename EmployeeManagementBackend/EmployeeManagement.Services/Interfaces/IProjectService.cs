using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Services.DTO.Project;

namespace EmployeeManagement.Services.Interfaces;

public interface IProjectService
{
    Task<PaginatedList<ProjectDetailDTO>> GetProjects(ProjectQueryParamater parameters);
    Task<ProjectDetailDTO> GetProjectById(int id);
    Task<ProjectDetailDTO?> AddProjectAsync(AddEditProjectDTO newProjectDTO);
    Task<ProjectDetailDTO?> EditProject(AddEditProjectDTO projectDto);
    Task DeleteProject(int id);

}
