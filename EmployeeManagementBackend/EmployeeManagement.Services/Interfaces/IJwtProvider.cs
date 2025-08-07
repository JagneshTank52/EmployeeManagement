using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Services.Interfaces;

public interface IJwtProvider
{
    public SecurityKey GetSigningKey(string algorithm);
    TokenValidationParameters GetParameters(bool validateLifetime);
}
