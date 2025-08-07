using System.Net;

namespace EmployeeManagement.Entities.Shared.ExceptionHandling;

public class SecurityTokenException : Exception
{
    public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.Unauthorized;
    public override string Message { get; }

    public SecurityTokenException(string message) : base(message) 
    {
        Message = message;
    }
 
    public SecurityTokenException(string message, HttpStatusCode code) : base(message)
    {
        Message = message;
        StatusCode = code;
    }
}
