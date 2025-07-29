using EmployeeManagement.Entities.Models;
using EmployeeManagement.Services.Auth.DTO;
using EmployeeManagement.Services.DTO.Auth;
using EmployeeManagement.Services.Implementations;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    /// Authenticates user and returns access/refresh tokens
    /// </summary>
    /// <param name="loginRequest">Login credentials</param>
    /// <returns>SuccessResponse with auth tokens or ErrorResponse if authentication fails</returns>
    [HttpPost("login")]
    public async Task<ActionResult<SuccessResponse<AuthResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);

        Response.Cookies.Append("RefreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return Ok(SuccessResponse<AuthResponseDTO>.Create(
            data: result,
            message: "Login successful"
        ));
    }

    /// <summary>
    /// Refreshes access token using refresh token from cookie
    /// </summary>
    /// <returns>SuccessResponse with new tokens or ErrorResponse if refresh fails</returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<SuccessResponse<AuthResponseDTO>>> RefreshToken()
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException("No refresh token found");
        }

        var result = await _authService.RefreshTokenAsync(refreshToken);

        Response.Cookies.Append("RefreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = result.ExpiresIn
        });

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
