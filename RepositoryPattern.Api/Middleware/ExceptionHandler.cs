using System.Net;
using System.Text.Json;

namespace RepositoryPattern.Api.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass control to the next middleware/component in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle exceptions globally
                _logger.LogError(ex, "An unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            // Default to 500
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            // Customize based on exception type
            switch (exception)
            {
                case ArgumentNullException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "A required argument was null.";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Resource not found.";
                    break;
                case Exception:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An internal server error has occurred.";
                    break;
                    // Add more exception types as needed
            }

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                statusCode = (int)statusCode
            });

            response.StatusCode = (int)statusCode;
            return response.WriteAsync(result);
        }
    }
}
