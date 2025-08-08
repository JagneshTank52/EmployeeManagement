using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO;

namespace EmployeeManagement.Services.Interfaces;

public interface IEmployeeService 
{
    public Task<PaginatedList<EmployeeDetailDTO>> GetEmployees(PaginationQueryParamater paramaters);
    public Task<EmployeeDetailDTO?> GetEmployeeById(int id);
    public Task<EmployeeDetailDTO?> AddEmployee(AddEmployeeDTO employeeDto);
    public Task<EmployeeDetailDTO?> UpdateEmployee(AddEmployeeDTO employeeDto);
    public Task DeleteEmployee(int id);

}
