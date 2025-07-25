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
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    public AuthenticationService(IConfiguration configuration, ITokenService tokenService, JwtSettings jwtSettings, IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _tokenService = tokenService;
        _jwtSettings = jwtSettings;
        _mapper = mapper;
        _config = configuration;
        _employeeRepository = employeeRepository;

    }
    public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmail(loginRequest.Email);

        if (employee == null || employee.Password != loginRequest.Password)
        {
            return ApiResponse<AuthResponseDTO>.ErrorResponse("Invalid email or password.");
        }

        var accessToken = _tokenService.GenerateAccessToken(employee);
        var refreshToken = _tokenService.GenerateRefreshToken();

        string actualToken = await _tokenService.SaveRefreshTokenAsync(employee.Id, refreshToken);

        double expireMinute;
        if (!double.TryParse(_config["JwtSettings:AccessTokenExpirationMinutes"], out expireMinute))
        {
            expireMinute = 2;
        }

        var expiresIn = DateTime.Now.AddMinutes(expireMinute);

        var authResponse = new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = actualToken,
            ExpiresIn = expiresIn,
        };

        return ApiResponse<AuthResponseDTO>.SuccessResponse(authResponse, "Login successful.");
    }

    public async Task<ApiResponse<bool>> LogoutAsync(string refreshToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(refreshToken);
        return ApiResponse<bool>.SuccessResponse(true, "Logout successful.");
    }

    public async Task<ApiResponse<AuthResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest)
    {
        RefreshToken? ValidRefreshToken = await _tokenService.ValidateRefreshTokenAsync(refreshTokenRequest.RefreshToken);

        if (ValidRefreshToken == null)
        {
            return ApiResponse<AuthResponseDTO>.ErrorResponse("Invalid or expired refresh token.");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(ValidRefreshToken.Employee);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        bool isUpdated = await _tokenService.UpdateRefreshToken(ValidRefreshToken, newRefreshToken);

        if (!isUpdated)
        {
            return ApiResponse<AuthResponseDTO>.ErrorResponse("Refresh token not generated");
        }

        double expireMinute;
        if (!double.TryParse(_config["JwtSettings:RefreshTokenExpirationDays"], out expireMinute))
        {
            expireMinute = 2;
        }

        var expiresIn = DateTime.Now.AddMinutes(expireMinute);

        var authResponse = new AuthResponseDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = expiresIn,
        };
        return ApiResponse<AuthResponseDTO>.SuccessResponse(authResponse, "Token refreshed successfully.");

    }

}
