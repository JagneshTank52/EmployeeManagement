using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.Project;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementations;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper, IProjectEmployeeRepository projectEmployeeRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _projectEmployeeRepository = projectEmployeeRepository;
    }

    public async Task<ProjectDetailDTO?> GetEmployeeById(int id)
    {
        Project? project = await _projectRepository.GetFirstOrDefaultAsync(filter: f => f.IsDeleted && f.Id == id,include: p => p.Include(q => q.ProjectEmployees).Include(s => s.Technology));

        if (project == null)
        {
            return null;
        }

        EmployeeDetailDTO employeeDetailDTO = _mapper.Map<EmployeeDetailDTO>(employee);
        return employeeDetailDTO;
    }

    public async Task<ProjectDetailDTO?> AddProjectAsync(AddEditProjectDTO newProjectDTO)
    {
        Project newProject = _mapper.Map<Project>(newProjectDTO);
        newProject.ModifiedBy = 3007;

        int projectId = await _projectRepository.AddAsync(newProject);

        List<ProjectEmployee> EmployeesAssignedToProject = new List<ProjectEmployee>();

        foreach (int employeeId in newProjectDTO.AssignedEmployeeIds)
        {
            ProjectEmployee projetEmployee = new ProjectEmployee
            {
                ProjectId = projectId,
                EmployeeId = employeeId,
                AssignedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                ModifiedBy = 3007,
                IsDeleted = false
            };  

            EmployeesAssignedToProject.Add(projetEmployee);
        }

        await _projectEmployeeRepository.AddRangeAsync(EmployeesAssignedToProject);



    }
}
