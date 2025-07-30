using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Entities.Shared.Constant;

namespace EmployeeManagement.Entities.Models
{
    public class SuccessResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public SuccessResponse(T? data,string message = "Operation completed successfully",int statusCode = (int)Enums.EmpStatusCode.Ok)
        {
            Success = true;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        public static SuccessResponse<T> Create(T data, string message = "Operation completed successfully", int statusCode = (int)Enums.EmpStatusCode.Ok)
        {
            return new SuccessResponse<T>(data, message, statusCode);
        }
    }
}
