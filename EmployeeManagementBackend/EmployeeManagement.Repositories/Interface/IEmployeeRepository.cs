using EmployeeManagement.Entities.Models;

namespace EmployeeManagement.Repositories.Interface;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    public Task<bool> EmployeeExistsByEmail(string email);
    // public Task<int> AddCustomer (Employee newEmployee);
}
