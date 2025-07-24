using EmployeeManagement.Entities.Models;

namespace EmployeeManagement.Repositories.Interface;

public interface IAuthenticationRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetRefreshToken(string refreshToken);
}
