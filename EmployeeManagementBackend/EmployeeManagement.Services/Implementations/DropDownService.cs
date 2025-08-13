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
    private readonly IMapper _mapper;

    public DropDownService(
        IGenericRepository<Technology> technologyRepository,
        IGenericRepository<Employee> employeeRepository,
        IGenericRepository<ProjectTaskStatus> taskStatusRepository,
        IMapper mapper
        )
    {
        _technologyRepository = technologyRepository;
        _taskStatusRepository = taskStatusRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<List<DropDownListDTO>> GetDropDownListsAsync(Enums.DropDownType dropDownType)
    {
        var dropDownList = await GetRepository(dropDownType);

        return dropDownList;
    }

    private async Task<List<DropDownListDTO>> GetRepository(Enums.DropDownType dropDownType)
    {
        return dropDownType switch
        {
            Enums.DropDownType.Technology =>
             _mapper.Map<List<DropDownListDTO>>(await _technologyRepository.GetAllAsync()),

            Enums.DropDownType.TaskStatus =>
                _mapper.Map<List<DropDownListDTO>>(await _taskStatusRepository.GetAllAsync()),

            Enums.DropDownType.Employee =>
                _mapper.Map<List<DropDownListDTO>>(await _employeeRepository.GetAllAsync()),


            _ => throw new ArgumentOutOfRangeException(nameof(dropDownType), dropDownType, null)
        };
    }

}
