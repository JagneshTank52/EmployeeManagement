using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(EmpManagementContext context) : base(context)
    {
    }

}
