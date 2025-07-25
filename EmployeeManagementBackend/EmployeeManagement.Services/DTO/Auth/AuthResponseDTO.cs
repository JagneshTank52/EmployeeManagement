namespace EmployeeManagement.Services.DTO.Auth;

public class AuthResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresIn { get; set; }
}
