using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Base controller providing common functionality for all API controllers.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IValidationService _validationService;

        protected BaseApiController(ILogger logger, IValidationService validationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        /// <summary>
        /// Creates a successful response with data.
        /// </summary>
        protected IActionResult SuccessResponse<T>(T? data, string message = "Success", int statusCode = 200)
        {
            return Ok(new ApiResponseDto<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        protected IActionResult ErrorResponse(string message, int statusCode = 500, string? errorCode = null)
        {
            return StatusCode(statusCode, new ApiResponseDto<string>
            {
                StatusCode = statusCode,
                Message = message,
                ErrorCode = errorCode,
                Data = null,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Creates a validation error response.
        /// </summary>
        protected IActionResult ValidationErrorResponse(string errors)
        {
            return BadRequest(new ApiResponseDto<string>
            {
                StatusCode = 400,
                Message = "Model validation failed.",
                ErrorCode = "VALIDATION_ERROR",
                Data = errors,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates the model state and returns an error response if invalid.
        /// </summary>
        protected bool ValidateModel(out string errors)
        {
            errors = string.Empty;
            if (ModelState.IsValid)
                return true;

            errors = _validationService.GetValidationErrors(ModelState);
            return false;
        }
    }
}
