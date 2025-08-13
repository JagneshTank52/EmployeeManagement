using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class TechnologyRepository : GenericRepository<Technology>, ITechnologyRepository
{
    public TechnologyRepository(EmpManagementContext context) : base(context) {}

}
