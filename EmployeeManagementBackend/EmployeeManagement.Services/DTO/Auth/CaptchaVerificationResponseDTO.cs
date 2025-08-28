using System.Text.Json.Serialization;

namespace EmployeeManagement.Services.DTO.Auth;

public class CaptchaVerificationResponseDTO
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("score")]   // v3 only
    public float? Score { get; set; }

    [JsonPropertyName("action")]  // v3 only
    public string? Action { get; set; }

    [JsonPropertyName("challenge_ts")]
    public DateTime? ChallengeTimeStamp { get; set; }

    [JsonPropertyName("hostname")]
    public string? HostName { get; set; }

    [JsonPropertyName("error-codes")]
    public List<string>? ErrorCodes { get; set; }
}
