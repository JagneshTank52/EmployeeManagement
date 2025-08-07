using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using EmployeeManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Services.Implementations;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;
    
    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenValidationParameters GetParameters(bool validateLifetime)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var alg = jwtToken.Header.Alg;

                return new[] { GetSigningKey(alg) };
            },
            ValidateIssuer = true,
            ValidIssuer = _configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["JwtSettings:Audience"],
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        };
    }

    public SecurityKey GetSigningKey(string algorithm)
    {
        return algorithm switch
        {
            SecurityAlgorithms.HmacSha256 =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:HmacKey"])),

            SecurityAlgorithms.RsaSha256 =>
                new RsaSecurityKey(LoadRsaPublicKeyFromPem(_configuration["JwtSettings:RsaPublicKey"]!)),

            SecurityAlgorithms.EcdsaSha256 =>
                new ECDsaSecurityKey(LoadEcdsaPublicKeyFromPem(_configuration["EcdsaPublicKey"]!)),

            _ => throw new Exception("Unsupported signing algorithm.")
        };

    }

    public static RSA LoadRsaPublicKeyFromPem(string pem)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem.ToCharArray());
        return rsa;
    }

    public static ECDsa LoadEcdsaPublicKeyFromPem(string pem)
    {
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(pem.ToCharArray());
        return ecdsa;
    }
}
