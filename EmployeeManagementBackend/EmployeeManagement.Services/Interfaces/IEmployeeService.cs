using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Models.QueryParamaterModel;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.DTO.DropDown;
using EmployeeManagement.Services.DTO.Employee;

namespace EmployeeManagement.Services.Interfaces;

public interface IEmployeeService 
{
    public Task<PaginatedList<EmployeeDetailDTO>> GetEmployees(PaginationQueryParamater paramaters);
    public Task<List<DropDownListDTO>> GetEmployeeSelectListAsync();
    public Task<EmployeeDetailDTO?> GetEmployeeById(int id);
    public Task<EmployeeDetailDTO?> AddEmployee(AddEmployeeDTO employeeDto);
    public Task<EmployeeDetailDTO?> UpdateEmployee(AddEmployeeDTO employeeDto);
    public Task DeleteEmployee(int id);

}
