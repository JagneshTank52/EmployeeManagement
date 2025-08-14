using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class WorklogRepository : GenericRepository<WorkLog>, IWorklogRepository
{
    public WorklogRepository(EmpManagementContext context) : base(context) {}

    

}
