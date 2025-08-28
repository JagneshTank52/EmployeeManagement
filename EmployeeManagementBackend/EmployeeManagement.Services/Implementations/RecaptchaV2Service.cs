using System.Text.Json;
using EmployeeManagement.Services.DTO.Auth;
using EmployeeManagement.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Implementations;

public class RecaptchaV2Service(IConfiguration configuration,HttpClient httpClient,ILogger<RecaptchaV2Service> logger) : IRecaptchaV2Service
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<RecaptchaV2Service> _logger = logger;
    private readonly string _secretKey = configuration["Captcha:SecretKey"] 
            ?? throw new ArgumentNullException("Captcha:SecretKey not configured");

    private readonly string _verificationUrl = configuration["Captcha:VerificationUrl"]!; 
           
    public async Task<bool> ValidateAsync(string token, string? remoteIp)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _verificationUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret", _secretKey },
                    { "response", token },
                    { "remoteip", remoteIp ?? "" }
                })
            };

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Captcha verification failed: HTTP {StatusCode}", response.StatusCode);
                return false;
            }

            var json = await response.Content.ReadAsStringAsync();
            var captchaResult = JsonSerializer.Deserialize<CaptchaVerificationResponseDTO>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (captchaResult?.Success != true)
            {
                _logger.LogWarning("Captcha verification rejected.");
                    
            }

            return captchaResult?.Success ?? false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during captcha verification.");
            return false;
        }
    }

}
