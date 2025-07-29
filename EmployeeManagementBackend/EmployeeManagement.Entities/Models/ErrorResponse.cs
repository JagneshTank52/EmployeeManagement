using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Models
{
    public class ErrorResponse : ApiResponse
    {
        public string? Error { get;set; }
        public object? Details { get; set; }

        public ErrorResponse(string error,string message = "Some error occur", int statusCode = 400, object? details = null)
        {
            Success = false;
            Error = error;
            Message = message;
            StatusCode = statusCode;
            Details = details;
        }

        public static ErrorResponse Create(string error, string message = "Some error occur", int statusCode = 400, object? details = null)
        {
            return new ErrorResponse(error, message, statusCode, details);
        }
    }
}
