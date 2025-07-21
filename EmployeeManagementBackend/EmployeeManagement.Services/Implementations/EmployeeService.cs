using System.Threading.Tasks;
using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Implementation;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository,IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }
    public async Task<List<EmployeeDetailDTO>> GetEmployees()
    {
        var employees = await _employeeRepository.GetAllAsync(include: i => i.Include(a => a.Department).Include(a => a.Role));

        List<EmployeeDetailDTO> employeeList = _mapper.Map<List<EmployeeDetailDTO>>(employees);

        return employeeList;
    }

    public async Task<EmployeeDetailDTO?> GetEmployeeById (int id){
        Employee? employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null){
            return null;
        } 

        EmployeeDetailDTO employeeDetailDTO = _mapper.Map<EmployeeDetailDTO>(employee);
        return employeeDetailDTO;
    }

    public async Task<EmployeeDetailDTO?> AddEmployee(AddEmployeeDTO employeeDto)
    {
        if ( await _employeeRepository.EmployeeExistsByEmail(employeeDto.Email))
        {
            return null;
        }

        Employee newEmployee = _mapper.Map<Employee>(employeeDto);
        newEmployee.CreatedAt = DateTime.UtcNow;
        newEmployee.HashPassword = employeeDto.Password;

        Employee addedEmployee = await _employeeRepository.AddAsync(newEmployee);

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
