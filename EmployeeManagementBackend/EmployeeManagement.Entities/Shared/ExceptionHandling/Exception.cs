using EmployeeManagement.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Shared.ExceptionHandling
{
    /// <summary>
    /// Data validation custom exception
    /// </summary>
    public class DataValidationException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public IList<ValidationDetailModel> Error { get; private set; }

        // Constructor for single validation error
        public DataValidationException(string propertyName, string errorMessage, string message = "") : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = new List<ValidationDetailModel>
            {
                new ValidationDetailModel
                {
                    InputName = propertyName,
                    ValidationMessage = errorMessage
                }
            };
        }
    }

    /// <summary>
    /// Data not found custom exception
    /// </summary>
    /// <param name="message"></param>
    public class DataNotFoundException(string message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.NotFound;
        public string Message { get; private set; } = message;
    }

    /// <summary>
    /// Data conflict custom exception
    /// </summary>
    /// <param name="message"></param>
    public class DataConflictException(string message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.Conflict;
        public string Message { get; private set; } = message;
    }

    public class HttpRequestFailedException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? ResponseContent { get; }

        public HttpRequestFailedException(string message, HttpStatusCode statusCode, string? responseContent = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }

    /// <summary>
    /// Custom exception for forbidden (403) access
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.Forbidden;
        public string Message { get; private set; }

        public ForbiddenAccessException(string message = "Access to the requested resource is forbidden.")
            : base(message)
        {
            Message = message;
        }
    }
}
