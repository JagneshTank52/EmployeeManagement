using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO;
using EmployeeManagement.Services.DTO.Auth;
using EmployeeManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    public AuthenticationService(IConfiguration configuration, ITokenService tokenService, IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _tokenService = tokenService;
        _mapper = mapper;
        _config = configuration;
        _employeeRepository = employeeRepository;

    }
    /// <summary>
    /// Authenticates user and returns authentication tokens
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>AuthResponseDTO with access and refresh tokens</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when credentials are invalid</exception>
    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmail(loginRequest.Email);

        if (employee == null || employee.Password != loginRequest.Password)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var accessToken = _tokenService.GenerateAccessToken(employee);
        var refreshToken = _tokenService.GenerateRefreshToken();
        string actualToken = await _tokenService.SaveRefreshTokenAsync(employee.Id, refreshToken);

        if (!double.TryParse(_config["JwtSettings:RefreshTokenExpirationDays"], out double expireDays))
        {
            expireDays = 7;
        }

        var expiresIn = DateTime.Now.AddDays(expireDays);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = actualToken,
            ExpiresIn = expiresIn,
        };
    }

    /// <summary>
    /// Logs out user by revoking refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token to revoke</param>
    /// <returns>True if logout successful</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when refresh token is invalid</exception>
    public async Task<bool> LogoutAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException("Refresh token is required for logout");
        }
        try
        {
            await _tokenService.RevokeRefreshTokenAsync(refreshToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to logout user", ex);
        }
    }

    /// <summary>
    /// Refreshes access token using valid refresh token
    /// </summary>
    /// <param name="refreshTokenRequest">Refresh token request</param>
    /// <returns>New AuthResponseDTO with updated tokens</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when refresh token is invalid or expired</exception>
    /// <exception cref="InvalidOperationException">Thrown when token refresh process fails</exception>
    public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
    {
        RefreshToken? ValidRefreshToken = await _tokenService.ValidateRefreshTokenAsync(refreshToken);

        if (ValidRefreshToken == null)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(ValidRefreshToken.Employee);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        bool isUpdated = await _tokenService.UpdateRefreshToken(ValidRefreshToken, newRefreshToken);

        if (!isUpdated)
        {
            throw new InvalidOperationException("Failed to update refresh token");
        }

        return new AuthResponseDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = ValidRefreshToken.ExpiryDate,
        };
    }

}
