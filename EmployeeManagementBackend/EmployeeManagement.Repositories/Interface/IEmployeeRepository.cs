using EmployeeManagement.Entities.Models;

namespace EmployeeManagement.Repositories.Interface;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    public Task<bool> EmployeeExistsByEmail(string email);
    public Task<Employee?> GetEmployeeByEmail(string email);
    Task<Employee?> AddEmployeeAsync(Employee entity);
}
