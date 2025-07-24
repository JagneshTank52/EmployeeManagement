using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Implementation;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IConfiguration _config;
    private readonly IAuthenticationRepository _authRepository;

    public TokenService(IOptions<JwtSettings> jwtSettings, IAuthenticationRepository authRepository, IConfiguration configuration)
    {
        _jwtSettings = jwtSettings.Value;
        _authRepository = authRepository;
        _config = configuration;
    }

    public string GenerateAccessToken(Employee employee)
    {
        var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.UserName ?? employee.Email),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role.RoleName),
                // new Claim(ClaimTypes.Uri,user.ProfileImage!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        double expireMinute;
        if (!double.TryParse(_config["JwtSettings:AccessTokenExpirationMinutes"], out expireMinute))
        {
            expireMinute = 10;
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinute),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        RefreshToken? token = await _authRepository.GetRefreshToken(refreshToken);

        if (token != null)
        {
            token.IsRevoked = true;
            token.ExpiryDate = DateTime.UtcNow;
            // Here handle null logic 
            await _authRepository.UpdateAsync(token);
        }
    }

    public async Task<string> SaveRefreshTokenAsync(int employeeId, string refreshToken)
    {
        double expireMinute;
        if (!double.TryParse(_config["JwtSettings:RefreshTokenExpirationDays"], out expireMinute))
        {
            expireMinute = 1;
        }

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            // note - here you can playwith time also
            ExpiryDate = DateTime.UtcNow.AddMinutes(expireMinute),
            IsRevoked = false,
            EmployeeId = employeeId,
            CreatedAt = DateTime.UtcNow
        };

        // Here corrct the logic if it is not found
        RefreshToken? token = await _authRepository.AddAsync(refreshTokenEntity);
        return token!.Token;
    }

    public async Task<bool> UpdateRefreshToken(RefreshToken refreshTokenEntity,string newRefreshToken){
        
        refreshTokenEntity.Token = newRefreshToken;

        RefreshToken? updatedRefreshToken = await _authRepository.UpdateAsync(refreshTokenEntity);

        if(updatedRefreshToken == null){
            return false;
        }

        return true;

    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken)
    {
        var token = await _authRepository.GetRefreshToken(refreshToken);

        if (token != null && token.ExpiryDate > DateTime.UtcNow)
        {
            return token;
        }

        return null;
    }

}
