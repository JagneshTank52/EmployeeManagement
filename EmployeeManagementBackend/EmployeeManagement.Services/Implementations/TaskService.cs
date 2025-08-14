using System.Linq.Expressions;
using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.Project;
using EmployeeManagement.Services.DTO.Task;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementations;

public class TaskService(ITaskRepository taskRepository, IMapper mapper) : ITaskService
{
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly IMapper _mapper = mapper;


    public async Task<PaginatedList<TaskDetailDTO>> GetTasksAsync(TaskQueryParameter parameters)
    {
        Expression<Func<ProjectTask, bool>> filter = p => !p.IsDeleted;

        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            var term = parameters.SearchTerm.ToLower();
            filter = filter.AndAlso(p =>
                p.Title.ToLower().Contains(term) ||
                p.Code!.ToLower().Contains(term)
            );
        }

        if (parameters.ProjectId.HasValue)
            filter = filter.AndAlso(p => p.ProjectId == parameters.ProjectId);

        if (parameters.StatusId.HasValue)
            filter = filter.AndAlso(p => p.StatusId == parameters.StatusId);

        if (parameters.AssignedTo.HasValue)
            filter = filter.AndAlso(p => p.AssignedTo == parameters.AssignedTo);

        if (!string.IsNullOrEmpty(parameters.Priority))
            filter = filter.AndAlso(p => p.Priority == parameters.Priority);

        Func<IQueryable<ProjectTask>, IOrderedQueryable<ProjectTask>> orderBy = parameters.SortBy switch
        {
            "name_asc" => q => q.OrderBy(p => p.Title),
            "name_desc" => q => q.OrderByDescending(p => p.Title),
            _ => q => q.OrderByDescending(p => p.Id)
        };

        Func<IQueryable<ProjectTask>, IQueryable<ProjectTask>> include = query => query.
            Include(a => a.Project).Include(b => b.Status).Include(c => c.AssignedToNavigation).Include(d => d.ReportedByNavigation);

        var projects = await _taskRepository.GetPagedRecords(
            parameters.PageSize,
            parameters.PageNumber,
            orderBy,
            filter,
            include
        );

        return new PaginatedList<TaskDetailDTO>(
            _mapper.Map<List<TaskDetailDTO>>(projects.records),
            projects.pageIndex,
            projects.pageSize,
            projects.totalRecord
        );
    }

    public async Task<TaskDetailDTO> GetTaskByIdAsync(int id)
    {
        ProjectTask task = await _taskRepository.GetFirstOrDefaultAsync(
            filter: f => !f.IsDeleted && f.Id == id,
            include: i => i.Include(a => a.Project).Include(b => b.Status).Include(c => c.AssignedToNavigation).Include(d => d.ReportedByNavigation))
            ?? throw new DataNotFoundException($"Task with ID {id} not found");

        return _mapper.Map<TaskDetailDTO>(task);
    }

    public async Task<TaskDetailDTO> AddTaskAsync(AddEditTaskDTO dto)
    {
        ProjectTask newTask = _mapper.Map<ProjectTask>(dto);
        newTask.ModifiedBy = 3007;

        ProjectTask task = await _taskRepository.AddEntityAsync(newTask);

        return await GetTaskByIdAsync(task.Id);
    }

    public async Task<TaskDetailDTO> EditTaskAsync(AddEditTaskDTO dto)
    {
        ProjectTask existingTask = await _taskRepository.GetFirstOrDefaultAsync(
            filter: f => !f.IsDeleted && f.Id == dto.Id,
            include: i => i.Include(a => a.Project).Include(b => b.Status).Include(c => c.AssignedToNavigation).Include(d => d.ReportedByNavigation))
            ?? throw new DataNotFoundException($"Task with ID {dto.Id} not found");

        if (existingTask == null) return null;

        _mapper.Map(dto, existingTask);
        ProjectTask? updatedTask = await _taskRepository.UpdateAsync(existingTask);

        return _mapper.Map<TaskDetailDTO>(updatedTask);
    }

    public async Task DeleteTaskAsync(int id)
    {
        ProjectTask? taskToDelete = await _taskRepository.GetByIdAsync(id);

        if (taskToDelete == null)
        {
            throw new DataNotFoundException($"Task with ID {id} not found");
        }

        taskToDelete.IsDeleted = true;
        taskToDelete.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.DeleteAsync(taskToDelete);
    }
}
