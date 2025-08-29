using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;

        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

                var apiResponse = new ApiResponse { Success = false, Message = "An unexpected error occurred." };
                var statusCode = StatusCodes.Status500InternalServerError;

                switch (ex)
                {
                    case InvalidOperationException invEx:
                        statusCode = StatusCodes.Status409Conflict;
                        apiResponse.Message = invEx.Message;
                        _logger.LogWarning("Conflict: {Message}", invEx.Message);
                        break;
                    case ArgumentException argEx:
                        statusCode = StatusCodes.Status400BadRequest;
                        apiResponse.Message = argEx.Message;
                        _logger.LogWarning("Invalid request: {Message}", argEx.Message);
                        break;
                    case UnauthorizedAccessException authEx:
                        statusCode = StatusCodes.Status401Unauthorized;
                        apiResponse.Message = "Unauthorized access.";
                        _logger.LogWarning("Unauthorized access: {Message}", authEx.Message);
                        break;
                    case KeyNotFoundException notFoundEx:
                        statusCode = StatusCodes.Status404NotFound;
                        apiResponse.Message = "Resource not found.";
                        _logger.LogWarning("Resource not found: {Message}", notFoundEx.Message);
                        break;
                    case ValidationException valEx:
                        statusCode = StatusCodes.Status400BadRequest;
                        apiResponse.Message = valEx.Message;
                        _logger.LogWarning("Validation error: {Message}", valEx.Message);
                        break;
                    default:
                        // Leaves the message generic for unknown or internal errors
                        break;
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(apiResponse);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new ApiResponse
            {
                Success = false,
                Message = "Validation Failed",
                Errors = exception.Errors.Select(error => (ValidationErrorDetail)error)
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }

        private static Task HandleCustomExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = []
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
