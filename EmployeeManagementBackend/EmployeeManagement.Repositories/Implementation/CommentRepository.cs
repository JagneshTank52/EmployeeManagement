using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;

namespace EmployeeManagement.Repositories.Implementation;

public class CommentRepository : GenericRepository<TaskComment>, ICommentRepository

{
    public CommentRepository(EmpManagementContext context) : base(context)
    {
    }

}
