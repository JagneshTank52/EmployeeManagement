using System.Text.Json;

namespace EmployeeManagement.Entities.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public int StatusCode { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, this.GetType());
    }
}
