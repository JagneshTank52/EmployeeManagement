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

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);
        if (!result.Success)
        {
            return Unauthorized(result);
        }

        Response.Cookies.Append("RefreshToken", result.Data!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> RefreshToken()
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Ok(ApiResponse<AuthResponseDTO>.ErrorResponse("No refresh token found"));

        var refreshRequest = new RefreshTokenRequestDto { RefreshToken = refreshToken };
        var result = await _authService.RefreshTokenAsync(refreshRequest);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        Response.Cookies.Append("RefreshToken", result.Data!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = result.Data!.ExpiresIn
        });
        
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<bool>>> Logout()
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Ok(ApiResponse<AuthResponseDTO>.ErrorResponse("No refresh token found"));

        var refreshRequest = new RefreshTokenRequestDto { RefreshToken = refreshToken };

        var result = await _authService.LogoutAsync(refreshRequest.RefreshToken);

        Response.Cookies.Delete("RefreshToken");

        if (!result.Success)
        {
            // Even if it fails, the client should proceed with local cleanup
            return BadRequest(result);
        }
        return Ok(result);
    }
}
