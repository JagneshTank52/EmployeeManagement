using System.Linq.Expressions;
using AutoMapper;
using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Repositories.Implementation;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.Worklog;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementations;

public class WorklogService(IWorklogRepository worklogRepository, IProjectRepository projectRepository, IMapper mapper) : IWorklogService
{
    private readonly IWorklogRepository _worklogRepository = worklogRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    private readonly IMapper _mapper = mapper;

    public async Task<WorklogDetailsDTO> AddWorkLogAsync(AddEditWorklogDTO newDto)
    {
        WorkLog newWorkLog = _mapper.Map<WorkLog>(newDto);

        WorkLog workLog = await _worklogRepository.AddEntityAsync(newWorkLog);

        return await GetWorkLogByIdAsync(workLog.Id);
    }

    public async Task DeleteWorkLogAsync(int id)
    {
        WorkLog? workLogToDelete = await _worklogRepository.GetByIdAsync(id);

        if (workLogToDelete == null)
        {
            throw new DataNotFoundException($"WorkLog with ID {id} not found");
        }

        workLogToDelete.IsDeleted = true;
        workLogToDelete.UpdatedAt = DateTime.UtcNow;

        await _worklogRepository.DeleteAsync(workLogToDelete);
    }

    public async Task<WorklogDetailsDTO> EditWorkLogAsync(AddEditWorklogDTO dto)
    {
        if (!dto.Id.HasValue) throw new DataValidationException("Id", "Id is required for update");

        var entity = await _worklogRepository.GetFirstOrDefaultAsync(filter: f => f.Id == dto.Id && !f.IsDeleted);

        if (entity == null) return null;

        _mapper.Map(dto, entity);

        WorkLog? updatedWorkLog = await _worklogRepository.UpdateAsync(entity);

        return await GetWorkLogByIdAsync(updatedWorkLog!.Id);
    }

    public async Task<WorklogDetailsDTO> GetWorkLogByIdAsync(int id)
    {
        WorkLog? workLog = await _worklogRepository.GetFirstOrDefaultAsync(
                filter: f => !f.IsDeleted && !f.Task.IsDeleted && f.Id == id,
                include: i => i.Include(a => a.Task)
                              .ThenInclude(b => b.AssignedToNavigation)
                              .Include(a => a.Task)
                              .ThenInclude(b => b.Status)
                   ?? throw new DataNotFoundException($"Work log with ID {id} not found"));

        return _mapper.Map<WorklogDetailsDTO>(workLog);
    }

    public async Task<PaginatedList<WorklogDetailsDTO>> GetWorkLogsAsync(WorklogQueryParamater parameters)
    {
        Expression<Func<WorkLog, bool>> filter = p => !p.IsDeleted;

        if (parameters.TaskId.HasValue)
            filter = filter.AndAlso(p => p.TaskId == parameters.TaskId);

        if (parameters.AssignedToId.HasValue)
            filter = filter.AndAlso(p => p.WorkDoneBy == parameters.AssignedToId);

        Func<IQueryable<WorkLog>, IOrderedQueryable<WorkLog>> orderBy = parameters.SortBy switch
        {
            "name_asc" => q => q.OrderBy(p => p.Id),
            "name_desc" => q => q.OrderByDescending(p => p.Id),
            _ => q => q.OrderByDescending(p => p.Id)
        };

        Func<IQueryable<WorkLog>, IQueryable<WorkLog>> include = query => query
            .Include(a => a.Task)
                              .ThenInclude(b => b.AssignedToNavigation)
                              .Include(a => a.Task)
                              .ThenInclude(b => b.Status);

        var result = await _worklogRepository.GetPagedRecords(
            parameters.PageSize,
            parameters.PageNumber,
            orderBy,
            filter,
            include
        );

        return new PaginatedList<WorklogDetailsDTO>(
            _mapper.Map<List<WorklogDetailsDTO>>(result.records),
            result.pageIndex,
            result.pageSize,
            result.totalRecord
        );
    }

    public async Task<WorkSheetDetailsDTO> GetWorkSheetAsync(int month, int year, int projectId)
    {
        Project? project = await _projectRepository.GetFirstOrDefaultAsync(
            filter: f => !f.IsDeleted && f.Id == projectId,
            include: i => i.Include(a => a.ProjectTasks).ThenInclude(a1 => a1.Status)
                            .Include(b => b.ProjectTasks).ThenInclude(b1 => b1.WorkLogs)
        );

        if (project == null)
            throw new DataNotFoundException($"Project with Id {projectId} not found.");

        var monthStart = new DateOnly(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        // Flatten worklogs
        var worklogs = project.ProjectTasks
            .SelectMany(t => t.WorkLogs)
            .Where(w => DateOnly.FromDateTime(w.CreatedAt) >= monthStart && DateOnly.FromDateTime(w.CreatedAt) <= monthEnd && !w.IsDeleted)
            .ToList();

        // Build all days in month
        var dailyLogs = Enumerable.Range(0, monthEnd.Day)
            .Select(offset =>
            {
                var date = monthStart.AddDays(offset);

                var dailyMinutes = (int) worklogs
                    .Where(w => DateOnly.FromDateTime(w.CreatedAt) == date)
                    .Sum(w => w.WorkTimeHours);

                var weekStart = date.AddDays(-(int)date.DayOfWeek);
                var weekEnd = weekStart.AddDays(6);

                var weeklyMinutes = (int) worklogs
                    .Where(w => DateOnly.FromDateTime(w.CreatedAt) >= weekStart && DateOnly.FromDateTime(w.CreatedAt) <= weekEnd)
                    .Sum(w => w.WorkTimeHours);

                return new DailyWorklogDetailsDTO
                {
                    AttendanceDate = date,
                    Day = date.DayOfWeek.ToString(),
                    DailyWorkLoginMinutes = dailyMinutes,
                    WeeklyTotalTimeInMinutes = weeklyMinutes
                };
            })
            .ToList();

        var result = _mapper.Map<WorkSheetDetailsDTO>(project);
        result.DailyWorkLogs = dailyLogs;

        return result;
    }

}

