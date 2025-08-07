using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.Convertor;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO.Auth;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates user and returns access and refresh tokens
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>SuccessResponse with auth tokens or ErrorResponse if authentication fails</returns>
    [HttpPost("login")]
    public async Task<ActionResult<SuccessResponse<AuthResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);

        return Ok( new SuccessResponse<AuthResponseDTO>(
            data: result,
            message: Messages.Success.General.AuthSuccess("Login")
        ));
    }

    /// <summary>
    /// Registers a new user with the provided registration data.
    /// </summary>
    /// <param name="registerRequest">The registration request DTO containing user details.</param>
    /// <returns>
    /// A 201 Created response with a success message if registration is successful.
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
    {
        await _authService.RegisterAsync(registerRequest);

        return Ok( new SuccessResponse<Object?>(
            data: null,
            message: Messages.Success.General.AuthSuccess("Register"),
            statusCode: (int)Enums.EmpStatusCode.Created
            ));
    }

    /// <summary>
    /// Refreshes access token using refresh token from cookie
    /// </summary>
    /// <returns>SuccessResponse with new tokens or ErrorResponse if refresh fails</returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<SuccessResponse<AuthResponseDTO>>> RefreshToken()
    {
        string? refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException(Messages.Error.Auth.TokenExpired);
        }

        var result = await _authService.RefreshTokenAsync(refreshToken);
        
        return Ok(SuccessResponse<AuthResponseDTO>.Create(
            data: result,
            message: "Token refreshed successfully"
        ));
    }

    /// <summary>
    /// Logs out user and invalidates refresh token
    /// </summary>
    /// <returns>SuccessResponse confirming logout or ErrorResponse if logout fails</returns>
    [HttpPost("logout")]
    public async Task<ActionResult<SuccessResponse<bool>>> Logout()
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException("No refresh token found");
        }

        bool result = await _authService.LogoutAsync(refreshToken);

        Response.Cookies.Delete("RefreshToken");

        return Ok(SuccessResponse<bool>.Create(
            data: result,
             message: "Logout successful"
        ));
    }
}
