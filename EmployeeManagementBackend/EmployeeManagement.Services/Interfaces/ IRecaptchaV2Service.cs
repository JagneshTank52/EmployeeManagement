namespace EmployeeManagement.Services.Interfaces;

public interface  IRecaptchaV2Service
{
    Task<bool> ValidateAsync(string token, string? remoteIp);
}
