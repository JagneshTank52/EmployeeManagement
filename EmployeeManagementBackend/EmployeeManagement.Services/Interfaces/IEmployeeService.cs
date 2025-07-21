using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO;

namespace EmployeeManagement.Services.Interfaces;

public interface IEmployeeService 
{
    public Task<List<EmployeeDetailDTO>> GetEmployees();
    public Task<EmployeeDetailDTO?> GetEmployeeById(int id);
    public Task<EmployeeDetailDTO?> AddEmployee(AddEmployeeDTO employeeDto);
    public Task<EmployeeDetailDTO?> UpdateEmployee(AddEmployeeDTO employeeDto);
    public Task DeleteEmployee(int id);

}
