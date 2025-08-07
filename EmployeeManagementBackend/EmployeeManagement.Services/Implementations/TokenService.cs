using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly IAuthenticationRepository _authRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly string[] _allowedAlgorithms = new[]
   {
        SecurityAlgorithms.HmacSha256,
        SecurityAlgorithms.RsaSha256,
        SecurityAlgorithms.EcdsaSha256
    };


    public TokenService(IAuthenticationRepository authRepository, IConfiguration configuration, IJwtProvider jwtProvider)
    {
        _authRepository = authRepository;
        _config = configuration;
        _jwtProvider = jwtProvider;
    }

    public string GenerateAccessToken(Employee employee)
    {
        var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.UserName ?? employee.Email),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role!.RoleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.TryParse(_config["JwtSettings:AccessTokenExpirationMinutes"], out double expireMinute) ? expireMinute : 10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateAccessToken(string token, bool validateLifetime = true)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException(Messages.Error.Exception.NullOrEmptyArgumentMessage);
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        TokenValidationParameters validationParameters = _jwtProvider.GetParameters(validateLifetime);

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken)
                throw new SecurityTokenException(Messages.Error.Exception.TokenSignatureInvalidMessage);

            if (!_allowedAlgorithms.Contains(jwtToken.Header.Alg, StringComparer.InvariantCultureIgnoreCase))
                throw new SecurityTokenException(Messages.Error.Exception.TokenAlgorithmNotSupportedMessage);

            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new SecurityTokenException(Messages.Error.Exception.TokenExpiredMessage);
        }
        catch (Exception ex) when (
            ex is SecurityTokenException ||
            ex is ArgumentException ||
            ex is FormatException
        )
        {
            throw new SecurityTokenException(Messages.Error.Exception.InvalidTokenMessage);
        }
        catch (Exception ex)
        {
            throw new Exception(Messages.Error.Exception.InternalServerErrorMessage);
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken)
    {
        var token = await _authRepository.GetRefreshToken(refreshToken);

        if (token != null && token.ExpiryDate > DateTime.UtcNow)
        {
            return token;
        }
        else
        {
            return null;
        }
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

    public async Task<bool> UpdateRefreshToken(RefreshToken refreshTokenEntity, string newRefreshToken)
    {

        refreshTokenEntity.Token = newRefreshToken;

        RefreshToken? updatedRefreshToken = await _authRepository.UpdateAsync(refreshTokenEntity);

        if (updatedRefreshToken == null)
        {
            return false;
        }
        return true;
    }

    public string GetUserIdFromToken(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.UserData)?.Value ?? string.Empty;
    }

}
