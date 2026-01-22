using SchoolAPI.DTOs;
using System.Text.Json;

namespace SchoolAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponseDto<string>
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occurred while processing your request.",
                Data = null
            };

            switch (exception)
            {
                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = 400;
                    response.Message = "Invalid argument provided.";
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "An error occurred processing your request.";
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "An unexpected error occurred.";
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
