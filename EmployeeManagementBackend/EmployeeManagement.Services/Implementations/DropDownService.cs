using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Repositories.Implementation;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO.DropDown;
using EmployeeManagement.Services.DTO.Employee;
using EmployeeManagement.Services.Interfaces;

namespace EmployeeManagement.Services.Implementations;

public class DropDownService : IDropDownService
{
    private readonly IGenericRepository<Technology> _technologyRepository;
    private readonly IGenericRepository<ProjectTaskStatus> _taskStatusRepository;
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IGenericRepository<Project> _projectRepository;
    private readonly IMapper _mapper;

    public DropDownService(
        IGenericRepository<Technology> technologyRepository,
        IGenericRepository<Employee> employeeRepository,
        IGenericRepository<ProjectTaskStatus> taskStatusRepository,
        IGenericRepository<Project> projectRepository,
        IMapper mapper
        )
    {
        _technologyRepository = technologyRepository;
        _taskStatusRepository = taskStatusRepository;
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<List<DropDownListDTO>> GetDropDownListsAsync(Enums.DropDownType dropDownType, int? filterId)
    {
        var dropDownList = await GetRepository(dropDownType, filterId);

        return dropDownList;
    }

    private async Task<List<DropDownListDTO>> GetRepository(Enums.DropDownType dropDownType, int? filterId)
    {
        return dropDownType switch
        {
            Enums.DropDownType.Technology =>
             _mapper.Map<List<DropDownListDTO>>(await _technologyRepository.GetAllAsync()),

            Enums.DropDownType.TaskStatus =>
                _mapper.Map<List<DropDownListDTO>>(await _taskStatusRepository.GetAllAsync()),

            Enums.DropDownType.Employee =>
                filterId.HasValue
                ? _mapper.Map<List<DropDownListDTO>>(
                    await _employeeRepository.GetAllAsync(filter: f => !f.IsDeleted && f.ProjectEmployeeEmployees.Any(pe => pe.ProjectId == filterId.Value))
                  )
                : _mapper.Map<List<DropDownListDTO>>(
                    await _employeeRepository.GetAllAsync()
                  ),

            Enums.DropDownType.Project =>
                _mapper.Map<List<DropDownListDTO>>(await _projectRepository.GetAllAsync()),


            _ => throw new ArgumentOutOfRangeException(nameof(dropDownType), dropDownType, null)
        };
    }

}
