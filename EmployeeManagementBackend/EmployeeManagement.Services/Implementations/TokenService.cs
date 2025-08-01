using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly IAuthenticationRepository _authRepository;

    public TokenService(IAuthenticationRepository authRepository, IConfiguration configuration)
    {
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
                new Claim(ClaimTypes.Role, employee.Role!.RoleName),
                // new Claim(ClaimTypes.Uri,user.ProfileImage!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.TryParse(_config["JwtSettings:AccessTokenExpirationMinutes"], out double expireMinute) ? expireMinute : 10 ),
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
            await _authRepository.UpdateAsync(token);
        }
    }

    public async Task SaveRefreshTokenAsync(int employeeId, string refreshToken)
    {
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(double.TryParse(_config["JwtSettings:RefreshTokenExpirationDays"], out double expireDays) ? expireDays : 7),
            IsRevoked = false,
            EmployeeId = employeeId,
            CreatedAt = DateTime.UtcNow
        };

        await _authRepository.AddAsync(refreshTokenEntity);
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
        }else{
            return null;
        }
    }

}
