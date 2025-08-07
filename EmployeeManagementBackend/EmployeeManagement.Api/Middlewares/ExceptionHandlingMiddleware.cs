using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using System.Diagnostics;
using System.Net;

namespace EmployeeManagement.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Invoke method for each api request to handle exception
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogWarning(uaEx, "Unauthorized access.");
                await HandleExceptionAsync(httpContext, uaEx, HttpStatusCode.Unauthorized);
            }
            catch (ForbiddenAccessException faEx) // 🔄 Added
            {
                _logger.LogWarning(faEx, "Access forbidden.");
                await HandleExceptionAsync(httpContext, faEx, HttpStatusCode.Forbidden); // 🔄 403
            }
            catch (DataValidationException dvEx)
            {
                _logger.LogWarning(dvEx, "Data validation failed.");
                await HandleExceptionAsync(httpContext, dvEx, HttpStatusCode.BadRequest);
            }
            catch (BadHttpRequestException brEx)
            {
                _logger.LogWarning(brEx, "Bad HTTP request.");
                await HandleExceptionAsync(httpContext, brEx, HttpStatusCode.BadRequest);
            }
            catch (DataConflictException dcEx)
            {
                _logger.LogWarning(dcEx, "Data conflict occurred.");
                await HandleExceptionAsync(httpContext, dcEx, HttpStatusCode.Conflict);
            }
            catch (DataNotFoundException dnEx)
            {
                _logger.LogWarning(dnEx, "Data not found.");
                await HandleExceptionAsync(httpContext, dnEx, HttpStatusCode.NotFound);
            }
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Invalid operation.");
                await HandleExceptionAsync(httpContext, ioEx, HttpStatusCode.BadRequest);
            }
            catch (HttpRequestFailedException hrEx)
            {
                _logger.LogWarning(hrEx, "HTTP request failed.");
                await HandleExceptionAsync(httpContext, hrEx, hrEx.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("Request processed in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Handle exception and return ErrorResponse
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="statusCode"></param>
        /// <returns>ErrorResponse with exception details</returns>
        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError
        )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var (error, message) = GetExceptionInfo(exception);

            var errorResponse = new ErrorResponse(
                errors: error,
                message: message,
                statusCode: (int)statusCode
            );

            await context.Response.WriteAsync(errorResponse.ToString());
        }

        /// <summary>
        /// Get exception information based on exception type
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Tuple of error type, message, and details</returns>
        private static (string? error, string message) GetExceptionInfo(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException uaEx => (
                    Messages.Error.Exception.UnauthorizedErrorMessage,
                    uaEx.Message ?? "Access denied"
                ),
                ForbiddenAccessException faEx => (
                   Messages.Error.Exception.ForbiddenAccessExceptionMessage ?? "Forbidden access",
                   faEx.Message ?? "You do not have access"
               ),
                BadHttpRequestException brEx => (
                    Messages.Error.Exception.BadRequestErrorMessage,
                    brEx.Message ?? "Bad request"
                ),
                DataNotFoundException dnEx => (
                    Messages.Error.Exception.DataNotFoundExceptionMessage,
                    dnEx.Message ?? "Requested data not found"
                ),
                DataConflictException dcEx => (
                    Messages.Error.Exception.DataConflictExceptionMessage,
                    dcEx.Message ?? "Data conflict occurred"
                ),
                InvalidOperationException ioEx => (
                    Messages.Error.Exception.InvalidOperationExceptionMessage,
                    ioEx.Message ?? "Invalid operation"
                ),
                HttpRequestFailedException hrEx => (
                    hrEx.ResponseContent,
                    hrEx.Message
                ),
                _ => (
                    Messages.Error.Exception.InternalServerErrorMessage ?? "Internal server error",
                    exception.Message ?? "An unexpected error occurred"
                )
            };
        }
    }
}
