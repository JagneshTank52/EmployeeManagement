using System.Linq.Expressions;
using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.Project;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProjectDetailDTO>> GetProjects(ProjectQueryParamater parameters)
    {
        Expression<Func<Project, bool>> filter = p => !p.IsDeleted;

        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            var term = parameters.SearchTerm.ToLower();
            filter = filter.AndAlso(p =>
                p.Name.ToLower().Contains(term) ||
                p.Code!.ToLower().Contains(term)
            );
        }

        if (parameters.TechnologyId.HasValue)
            filter = filter.AndAlso(p => p.TechnologyId == parameters.TechnologyId);

        if (!string.IsNullOrEmpty(parameters.ProjectStatus))
            filter = filter.AndAlso(p => p.ProjectStatus == parameters.ProjectStatus);

        if (!string.IsNullOrEmpty(parameters.Type))
            filter = filter.AndAlso(p => p.Type == parameters.Type);

        Func<IQueryable<Project>, IOrderedQueryable<Project>> orderBy = parameters.SortBy switch
        {
            "name_asc" => q => q.OrderBy(p => p.Name),
            "name_desc" => q => q.OrderByDescending(p => p.Name),
            _ => q => q.OrderByDescending(p => p.Id)
        };

        Func<IQueryable<Project>, IQueryable<Project>> include = query => query
            .Include(p => p.Technology)
            .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee);

        var projects = await _projectRepository.GetPagedRecords(
            parameters.PageSize,
            parameters.PageNumber,
            orderBy,
            filter,
            include
        );

        return new PaginatedList<ProjectDetailDTO>(
            _mapper.Map<List<ProjectDetailDTO>>(projects.records),
            projects.pageIndex,
            projects.pageSize,
            projects.totalRecord
        );
    }

    public async Task<ProjectDetailDTO> GetProjectById(int id)
    {
        Project project = await _projectRepository.GetFirstOrDefaultAsync(
            filter: p => !p.IsDeleted && p.Id == id,
            include: p => p.Include(q => q.ProjectEmployees.Where(pe => !pe.IsDeleted)).ThenInclude(e => e.Employee)
                           .Include(t => t.Technology))
            ?? throw new DataNotFoundException($"Project with ID {id} not found");

        return _mapper.Map<ProjectDetailDTO>(project);
    }

    public async Task<ProjectDetailDTO?> AddProjectAsync(AddEditProjectDTO newProjectDTO)
    {
        Project newProject = _mapper.Map<Project>(newProjectDTO);
        newProject.ModifiedBy = 3007;

        newProject.ProjectEmployees = newProjectDTO.AssignedEmployeeIds
        .Select(id => new ProjectEmployee
        {
            EmployeeId = id,
            AssignedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            ModifiedBy = 3007,
            IsDeleted = false
        }).ToList();

        Project project = await _projectRepository.AddEntityAsync(newProject);

        return await GetProjectById(project.Id);
    }

    public async Task<ProjectDetailDTO?> EditProject(AddEditProjectDTO projectDto)
    {
        Project? existingProject = await _projectRepository.GetFirstOrDefaultAsync(filter: f => !f.IsDeleted && f.Id == projectDto.Id, include: q => q.Include(i => i.ProjectEmployees).ThenInclude(t => t.Employee).Include(a => a.Technology));

        if (existingProject == null) return null;

        // Existing Employees
        var existingEmployeeIds = existingProject.ProjectEmployees
            .Select(pe => pe.EmployeeId)
            .ToList();

        // updated Employee
        var newEmployeeIds = projectDto.AssignedEmployeeIds;

        // Employee to add
        var employeesToAdd = newEmployeeIds
            .Except(existingEmployeeIds)
            .ToList();

        // Employee to remove
        var employeesToRemove = existingEmployeeIds
            .Except(newEmployeeIds)
            .ToList();

        foreach (var employeeId in employeesToAdd)
        {
            existingProject.ProjectEmployees.Add(new ProjectEmployee
            {
                EmployeeId = employeeId,
                AssignedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                ModifiedBy = 3007,
                IsDeleted = false
            });
        }

        foreach (var projectEmployee in existingProject.ProjectEmployees
            .Where(pe => employeesToRemove.Contains(pe.EmployeeId)))
        {
            projectEmployee.IsDeleted = true;
            projectEmployee.UpdatedAt = DateTime.UtcNow;
        }

        // Map updated fields
        _mapper.Map(projectDto, existingProject, opt => opt.AfterMap((src, dest) =>
        {
            dest.UpdatedAt = DateTime.UtcNow;
        }));

        Project? updatedProject = await _projectRepository.UpdateAsync(existingProject);
        return _mapper.Map<ProjectDetailDTO>(updatedProject);
    }

    public async Task DeleteProject(int id)
    {
        Project? projectToDelete = await _projectRepository.GetByIdAsync(id);

        if (projectToDelete == null)
        {
            throw new DataNotFoundException($"Project with ID {id} not found");
        }

        projectToDelete.IsDeleted = true;
        projectToDelete.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.DeleteAsync(projectToDelete);
    }

}
