using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class TaskRepository : GenericRepository<ProjectTask>, ITaskRepository
{
    public TaskRepository(EmpManagementContext context) : base(context)
    {
    }

}
