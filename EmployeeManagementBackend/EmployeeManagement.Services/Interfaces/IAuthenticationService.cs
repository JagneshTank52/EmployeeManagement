using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO.Auth;

namespace EmployeeManagement.Services.Interfaces;

public interface IAuthenticationService
{
    Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginRequest);
    Task<ApiResponse<AuthResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest);

    Task<ApiResponse<bool>> LogoutAsync(string refreshToken);
}
