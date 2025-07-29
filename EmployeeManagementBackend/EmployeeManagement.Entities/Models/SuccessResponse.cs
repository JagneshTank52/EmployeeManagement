using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Models
{
    public class SuccessResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public SuccessResponse(T data,string message = "Operation completed successfully",int statusCode = 200)
        {
            Success = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        public static SuccessResponse<T> Create(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new SuccessResponse<T>(data, message, statusCode);
        }
    }
}
