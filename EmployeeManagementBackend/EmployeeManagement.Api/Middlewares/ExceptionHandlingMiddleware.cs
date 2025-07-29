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

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

            var (error, message, details) = GetExceptionInfo(exception);

            // Create ErrorResponse using inheritance
            var errorResponse = ErrorResponse.Create(
                error: error,
                message: message,
                statusCode: (int)statusCode,
                details: details
            );

            await context.Response.WriteAsync(errorResponse.ToString());
        }

        /// <summary>
        /// Get exception information based on exception type
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Tuple of error type, message, and details</returns>
        private static (string error, string? message, object? details) GetExceptionInfo(Exception exception)
        {
            return exception switch
            {
                DataValidationException dvEx => (
                    Messages.Error.Exception.DataValidationErrorMessage,
                    "One or more validation errors occurred",
                    dvEx.Error
                ),
                UnauthorizedAccessException uaEx => (
                    Messages.Error.Exception.UnauthorizedErrorMessage,
                    uaEx.Message ?? "Access denied",
                    null
                ),
                BadHttpRequestException brEx => (
                    Messages.Error.Exception.BadRequestErrorMessage,
                    brEx.Message ?? "Bad request",
                    null
                ),
                DataNotFoundException dnEx => (
                    Messages.Error.Exception.DataNotFoundExceptionMessage,
                    dnEx.Message ?? "Requested data not found",
                    null
                ),
                DataConflictException dcEx => (
                    Messages.Error.Exception.DataConflictExceptionMessage,
                    dcEx.Message ?? "Data conflict occurred",
                    null
                ),
                InvalidOperationException ioEx => (
                    Messages.Error.Exception.InvalidOperationExceptionMessage,
                    ioEx.Message ?? "Invalid operation",
                    null
                ),
                HttpRequestFailedException hrEx => (
                    "HTTP request failed",
                    hrEx.Message,
                    hrEx.ResponseContent
                ),
                _ => (
                    Messages.Error.Exception.InternalServerErrorMessage ?? "Internal server error",
                    exception.Message ?? "An unexpected error occurred",
                    null
                )
            };
        }
    }
}
