using EmployeeManagement.Entities.Models;

namespace EmployeeManagement.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Employee employee);
    string GenerateRefreshToken();
    Task<string> SaveRefreshTokenAsync(int employeeId, string refreshToken);
    Task<bool> UpdateRefreshToken(RefreshToken refreshTokenEntity, string newRefreshToken);
    Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}
