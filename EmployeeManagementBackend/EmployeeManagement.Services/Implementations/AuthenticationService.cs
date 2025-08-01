using AutoMapper;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO.Auth;
using EmployeeManagement.Services.Helpers;
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

    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmail(loginRequest.Email);

        if (employee == null || !PasswordHelper.VerifyPassword(loginRequest.Password, employee.HashPassword))
        {
            throw new UnauthorizedAccessException(Messages.Error.Auth.InvalidPasswordMessage);
        }

        var accessToken = _tokenService.GenerateAccessToken(employee);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _tokenService.SaveRefreshTokenAsync(employee.Id, refreshToken);

        var expiresIn = DateTime.Now.AddDays(double.TryParse(_config["JwtSettings:RefreshTokenExpirationDays"], out var days) ? days : 7);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = expiresIn,
        };
    }

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

    public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
    {
        RefreshToken? ValidRefreshToken = await _tokenService.ValidateRefreshTokenAsync(refreshToken);

        if (ValidRefreshToken == null)
        {
            throw new UnauthorizedAccessException(Messages.Error.Auth.InvalidJwtRefreshToken);
        }

        string newAccessToken = _tokenService.GenerateAccessToken(ValidRefreshToken.Employee);
        string newRefreshToken = _tokenService.GenerateRefreshToken();

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

    public async Task RegisterAsync(RegisterRequestDTO registerRequest)
    {
        Employee? existingUser = await _employeeRepository.GetFirstOrDefaultAsync(f => !f.IsDeleted && f.Email == registerRequest.Email);

        if (existingUser != null)
        {
            string errorMessage = Messages.Error.General.AlreadyExistsWithAttributeMessage(
            "Employee", "Email", registerRequest.Email);

            throw new DataConflictException(errorMessage);
        }

        Employee newEmployee = _mapper.Map<Employee>(registerRequest);
        newEmployee.HashPassword = PasswordHelper.HashPassword(registerRequest.Password);

        await _employeeRepository.AddAsync(newEmployee);
    }
}
