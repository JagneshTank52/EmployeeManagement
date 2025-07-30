using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementation;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<EmployeeDetailDTO>> GetEmployees(PaginationQueryParamater paramaters)
    {
        // 1 DEFAULT FILTER
        Expression<Func<Employee, bool>> employeeFilter = f => !f.IsDeleted;

        // 2 SEARCH FILTER
        if (!string.IsNullOrEmpty(paramaters.SearchTerm))
        {
            employeeFilter = employeeFilter.AndAlso(f => f.FirstName!.ToLower().Contains(paramaters.SearchTerm.ToLower())
                                    || f.LastName!.ToLower().Contains(paramaters.SearchTerm.ToLower())
                                    || f.Email.ToLower().Contains(paramaters.SearchTerm.ToLower()));
        }

        // 3 Order by
        Func<IQueryable<Employee>, IOrderedQueryable<Employee>> employeeOrderBy = paramaters.SortBy switch
        {
            "name_asc" => q => q.OrderBy(o => o.FirstName),
            _ => q => q.OrderBy(o => o.Id)
        };

        // 4 Include 
        Func<IQueryable<Employee>, IQueryable<Employee>>? employeeInclude = i => i.Include(a => a.Department).Include(b => b.Role);

        var employees = await _employeeRepository.GetPagedRecords(pageSize: paramaters.PageSize, pageIndex: paramaters.PageNumber, filter: employeeFilter, include: employeeInclude, orderBy: employeeOrderBy);

        var mappedItems = _mapper.Map<List<EmployeeDetailDTO>>(employees.records);

        PaginatedList<EmployeeDetailDTO> employeeList = new PaginatedList<EmployeeDetailDTO>(
            mappedItems,
            employees.pageIndex,
            employees.pageSize,
            employees.totalRecord
        );
        
        return employeeList;
    }

    public async Task<EmployeeDetailDTO?> GetEmployeeById(int id)
    {
        Employee? employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return null;
        }

        EmployeeDetailDTO employeeDetailDTO = _mapper.Map<EmployeeDetailDTO>(employee);
        return employeeDetailDTO;
    }

    public async Task<EmployeeDetailDTO?> AddEmployee(AddEmployeeDTO employeeDto)
    {
        if (await _employeeRepository.EmployeeExistsByEmail(employeeDto.Email))
        {
            return null;
        }

        Employee newEmployee = _mapper.Map<Employee>(employeeDto);
        newEmployee.CreatedAt = DateTime.UtcNow;
        newEmployee.HashPassword = employeeDto.Password;

        Employee? addedEmployee = await _employeeRepository.AddEmployeeAsync(newEmployee);

        return (
            _mapper.Map<EmployeeDetailDTO>(addedEmployee)
        );
    }

    public async Task<EmployeeDetailDTO?> UpdateEmployee(AddEmployeeDTO employeeDto)
    {
        Employee? existingEmployee = await _employeeRepository.GetByIdAsync(employeeDto.Id);

        if (existingEmployee == null)
        {
            return null;
        }

        if (existingEmployee.Email != employeeDto.Email && await _employeeRepository.EmployeeExistsByEmail(employeeDto.Email))
        {
            return null; // Another employee already has this email
        }

        _mapper.Map(employeeDto, existingEmployee);
        existingEmployee.UpdatedAt = DateTime.UtcNow;
        existingEmployee.HashPassword = employeeDto.Password;

        Employee? updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);

        return (
            _mapper.Map<EmployeeDetailDTO>(updatedEmployee)
        );
    }

    public async Task DeleteEmployee(int id)
    {
        Employee? employeeToDelete = await _employeeRepository.GetByIdAsync(id);

        employeeToDelete!.IsDeleted = true;
        employeeToDelete.UpdatedAt = DateTime.Now;

        await _employeeRepository.DeleteAsync(employeeToDelete);
    }
}
