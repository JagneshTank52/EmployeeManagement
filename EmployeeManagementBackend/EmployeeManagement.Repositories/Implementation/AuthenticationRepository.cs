using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories.Implementation;

public class AuthenticationRepository : GenericRepository<RefreshToken>, IAuthenticationRepository
{
    public AuthenticationRepository(EmpManagementContext context) : base(context) { }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
    {
        var token = await _context.RefreshTokens.Include(i => i.Employee).ThenInclude(t => t.Role)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        return token;
    }

}
