using System.Data;
using System.Threading.Tasks;
using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement.Repositories.Implementation;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
 
    public EmployeeRepository(EmpManagementContext context) : base(context){}

    public override async Task<Employee?> AddAsync(Employee entity)
    {
        Employee? addedEmployee = await base.AddAsync(entity);

        if(addedEmployee == null)
        {
            return null;
        }
        
        var employeeWithDetails = await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == addedEmployee.Id && !e.IsDeleted);

        return employeeWithDetails!;
    }
    public override async Task<Employee?> UpdateAsync(Employee entity, Func<Employee, bool> checkUniquePredicate = null)
    {
        Employee? updatedEmployee = await base.UpdateAsync(entity, checkUniquePredicate);
        
        if (updatedEmployee == null)
        {
            return null;
        }

        var employeeWithDetails = await _dbSet
            .Include(e => e.Department) 
            .Include(e => e.Role)      
            .FirstOrDefaultAsync(e => e.Id == updatedEmployee.Id && !e.IsDeleted);

        return employeeWithDetails;
    }

    public override async Task<Employee?> GetByIdAsync(int id)
    {
        Employee? employee = await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        return employee;
    }

    public async Task<bool> EmployeeExistsByEmail(string email)
    {
        return await _context.Employees.AnyAsync(e => e.Email == email);
    }

    public Task<Employee?> GetEmployeeByEmail(string email)
    {
        // here you need to also include department
        return _context.Employees.Include(i => i.Role).Include(i => i.Department).FirstOrDefaultAsync(f => f.Email == email && !f.IsDeleted);
    }
}
