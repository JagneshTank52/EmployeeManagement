using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO.Auth;

namespace EmployeeManagement.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequest);

    Task RegisterAsync(RegisterRequestDTO request);

    Task<bool> LogoutAsync(string refreshToken);

    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
}
