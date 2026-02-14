using SchoolAPI.DTOs;
using SchoolAPI.Exceptions;
using System.Text.Json;

namespace SchoolAPI.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred at {Path}", context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponseDto<string>
            {
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case ValidationException valEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = 400;
                    response.Message = valEx.Message;
                    response.ErrorCode = "VALIDATION_ERROR";
                    break;

                case NotFoundException notFoundEx:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.StatusCode = 404;
                    response.Message = notFoundEx.Message;
                    response.ErrorCode = "NOT_FOUND";
                    break;

                case UnauthorizedException unAuthEx:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.StatusCode = 401;
                    response.Message = unAuthEx.Message;
                    response.ErrorCode = "UNAUTHORIZED";
                    break;

                case ForbiddenException forbiddenEx:
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    response.StatusCode = 403;
                    response.Message = forbiddenEx.Message;
                    response.ErrorCode = "FORBIDDEN";
                    break;

                case SchoolApiException schoolEx:
                    context.Response.StatusCode = schoolEx.StatusCode;
                    response.StatusCode = schoolEx.StatusCode;
                    response.Message = schoolEx.Message;
                    response.ErrorCode = schoolEx.ErrorCode;
                    break;

                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = 400;
                    response.Message = "Invalid argument provided.";
                    response.ErrorCode = "INVALID_ARGUMENT";
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "An error occurred processing your request.";
                    response.ErrorCode = "INVALID_OPERATION";
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "An unexpected error occurred. Please try again later.";
                    response.ErrorCode = "INTERNAL_SERVER_ERROR";
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}

