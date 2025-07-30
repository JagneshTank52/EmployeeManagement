using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO.Auth;

namespace EmployeeManagement.Services.Interfaces;

public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates user and returns authentication tokens
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>AuthResponseDTO with access and refresh tokens</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when credentials are invalid</exception>
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequest);

    Task RegisterAsync(RegisterRequestDTO request);

    /// <summary>
    /// Logs out user by revoking refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token to revoke</param>
    /// <returns>True if logout successful</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when refresh token is invalid</exception>
    Task<bool> LogoutAsync(string refreshToken);

    /// <summary>
    /// Refreshes access token using valid refresh token
    /// </summary>
    /// <param name="refreshTokenRequest">Refresh token request</param>
    /// <returns>New AuthResponseDTO with updated tokens</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when refresh token is invalid or expired</exception>
    /// <exception cref="InvalidOperationException">Thrown when token refresh process fails</exception>
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
}
