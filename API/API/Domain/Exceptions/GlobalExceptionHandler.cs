using API.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    /// <summary>
    /// Global exception handler that catches all exceptions and returns appropriate ProblemDetails responses
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            var problemDetails = CreateProblemDetails(httpContext, exception);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        private ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var title = GetTitle(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Type = GetType(exception)
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            // Add validation errors if it's a ValidationException
            if (exception is ValidationException validationException && validationException.Errors.Any())
            {
                problemDetails.Extensions.Add("errors", validationException.Errors);
            }

            // Add exception code if available
            if (exception is DomainException domainException)
            {
                problemDetails.Extensions.Add("code", domainException.Code);
            }


            // Add stack trace in development environment
            if (_environment.IsDevelopment())
            {
                problemDetails.Extensions.Add("stackTrace", exception.StackTrace);
            }

            return problemDetails;
        }

        private static int GetStatusCode(Exception exception) => exception switch
        {
            // Domain Exceptions
            ValidationException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            EntityAlreadyExistsException => StatusCodes.Status409Conflict,
            EntityDeletedException => StatusCodes.Status410Gone,
            InvalidOperationDomainException => StatusCodes.Status400BadRequest,
            DomainException => StatusCodes.Status400BadRequest,



            // System Exceptions
            ArgumentException => StatusCodes.Status400BadRequest,

            InvalidOperationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            // Default
            _ => StatusCodes.Status500InternalServerError
        };

        private static string GetTitle(Exception exception) => exception switch
        {
            // Domain Exceptions
            ValidationException => "Validation Error",
            EntityNotFoundException => "Resource Not Found",
            EntityAlreadyExistsException => "Resource Already Exists",
            EntityDeletedException => "Resource Deleted",
            InvalidOperationDomainException => "Invalid Operation",
            DomainException => "Domain Error",



            // System Exceptions
            ArgumentException => "Invalid Argument",

            InvalidOperationException => "Invalid Operation",
            UnauthorizedAccessException => "Unauthorized Access",

            // Default
            _ => "Internal Server Error"
        };

        private static string GetType(Exception exception) => exception switch
        {
            // Domain Exceptions
            ValidationException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            EntityNotFoundException => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            EntityAlreadyExistsException => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            EntityDeletedException => "https://tools.ietf.org/html/rfc7231#section-6.5.10",
            InvalidOperationDomainException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            DomainException => "https://tools.ietf.org/html/rfc7231#section-6.5.1",



            // Default
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}
