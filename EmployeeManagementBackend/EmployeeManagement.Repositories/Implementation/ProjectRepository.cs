using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(EmpManagementContext context) : base(context) {}
}
