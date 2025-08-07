using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class ProjectEmployeeRepository : GenericRepository<ProjectEmployee>, IProjectEmployeeRepository
{
    public ProjectEmployeeRepository(EmpManagementContext context) : base(context) {}

}
