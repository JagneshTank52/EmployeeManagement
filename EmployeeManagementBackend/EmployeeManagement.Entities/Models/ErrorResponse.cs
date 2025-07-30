using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Models
{
    public class ErrorResponse : ApiResponse
    {
        public object? Errors { get;set; }

        public ErrorResponse(string message = "Some error occur", int statusCode = 400, object? errors = null)
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
