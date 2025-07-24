using System.ComponentModel.DataAnnotations;
namespace EmployeeManagement.Services.Auth.DTO;

public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }