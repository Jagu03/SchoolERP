using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities.Authentication;
using SchoolInfrastructure.Services;
using System.Text;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Authentication and authorization controller for user login/logout operations.
    /// </summary>
    [AllowAnonymous]
    public class AuthenticationController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        /// <summary>
        /// Authenticates user with login credentials and returns JWT tokens.
        /// </summary>
        /// <param name="request">Login request with credentials</param>
        /// <returns>Login response with user details and tokens</returns>
        /// <response code="200">Login successful</response>
        /// <response code="401">Invalid credentials</response>
        /// <response code="403">Account locked or no permission</response>
        /// <response code="500">Server error</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid login request model. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                // Extract client information from HttpContext
                var ipAddress = ExtractClientIpAddress();
                var (browser, browserVersion) = ExtractBrowserInfo();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var sessionId = AuthenticationService.GenerateSessionId();

                // Encode password to Base64 (stored procedure expects Base64 encoded password)
                var encodedPassword = EncodePasswordToBase64(request.UserPassword);

                // Call authentication service with null for optional parameters
                var response = await _authenticationService.LoginAsync(
                    request.LoginCode,
                    encodedPassword,
                    moduleName: null,              // ? Optional: always null from minimal request
                    userAuthCode: null,            // ? Optional: always null from minimal request
                    ipAddress,
                    sessionId,
                    browser,
                    browserVersion,
                    userAgent
                );

                if (response.StatusCode == 200)
                {
                    _logger.LogInformation("User logged in successfully. LoginCode: {LoginCode}", request.LoginCode);
                    return Ok(response);
                }

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user: {LoginCode}", request.LoginCode);
                return ErrorResponse("An unexpected error occurred during authentication", 500, "AUTH_ERROR");
            }
        }

        /// <summary>
        /// Logs out the authenticated user and invalidates session.
        /// </summary>
        /// <param name="request">Logout request with user and session info</param>
        /// <returns>Logout confirmation</returns>
        /// <response code="200">Logout successful</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid logout request. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _authenticationService.LogoutAsync(request.UserId, request.SessionId);

                if (result)
                {
                    _logger.LogInformation("User logged out. UserId: {UserId}", request.UserId);
                    return SuccessResponse(new { message = "Logout successful" }, "User logged out successfully");
                }

                return ErrorResponse("Logout failed", 400, "LOGOUT_FAILED");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for UserId: {UserId}", request.UserId);
                return ErrorResponse("An error occurred during logout", 500, "LOGOUT_ERROR");
            }
        }

        /// <summary>
        /// Refreshes access token using refresh token.
        /// </summary>
        /// <param name="request">Refresh token request</param>
        /// <returns>New access and refresh tokens</returns>
        /// <response code="200">Token refresh successful</response>
        /// <response code="400">Invalid refresh token</response>
        /// <response code="401">Token expired or invalid</response>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid refresh token request. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var response = await _authenticationService.RefreshTokenAsync(request.RefreshToken);

                _logger.LogInformation("Token refreshed successfully");
                return SuccessResponse(response, "Token refreshed successfully");
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Invalid refresh token provided");
                return ErrorResponse("Invalid or expired refresh token", 401, "INVALID_REFRESH_TOKEN");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return ErrorResponse("An error occurred while refreshing token", 500, "TOKEN_REFRESH_ERROR");
            }
        }

        /// <summary>
        /// Gets the authenticated user's profile information.
        /// </summary>
        /// <returns>User profile details</returns>
        /// <response code="200">User profile retrieved</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        [HttpGet("profile/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                var profile = await _authenticationService.GetUserProfileAsync(userId);

                if (profile == null)
                {
                    _logger.LogWarning("User profile not found. UserId: {UserId}", userId);
                    return ErrorResponse("User profile not found", 404, "PROFILE_NOT_FOUND");
                }

                _logger.LogInformation("User profile retrieved. UserId: {UserId}", userId);
                return SuccessResponse(profile, "User profile retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user profile for UserId: {UserId}", userId);
                return ErrorResponse("An error occurred while retrieving user profile", 500, "PROFILE_ERROR");
            }
        }

        /// <summary>
        /// Health check endpoint for authentication service.
        /// </summary>
        /// <returns>Service status</returns>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "OK", service = "Authentication Service", timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Extracts client IP address from HTTP context.
        /// </summary>
        private string ExtractClientIpAddress()
        {
            if (HttpContext?.Connection?.RemoteIpAddress == null)
                return "0.0.0.0";

            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            // Check for X-Forwarded-For header (proxy)
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    ipAddress = forwardedFor.Split(',')[0].Trim();
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Extracts browser name and version from user agent.
        /// </summary>
        private (string browser, string version) ExtractBrowserInfo()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(userAgent))
                return ("Unknown", "Unknown");

            // Simple browser detection
            if (userAgent.Contains("Chrome", StringComparison.OrdinalIgnoreCase))
                return ("Chrome", ExtractVersion(userAgent, "Chrome"));
            if (userAgent.Contains("Firefox", StringComparison.OrdinalIgnoreCase))
                return ("Firefox", ExtractVersion(userAgent, "Firefox"));
            if (userAgent.Contains("Safari", StringComparison.OrdinalIgnoreCase))
                return ("Safari", ExtractVersion(userAgent, "Safari"));
            if (userAgent.Contains("Edge", StringComparison.OrdinalIgnoreCase))
                return ("Edge", ExtractVersion(userAgent, "Edge"));
            if (userAgent.Contains("Trident", StringComparison.OrdinalIgnoreCase))
                return ("IE", "11");

            return ("Unknown", "Unknown");
        }

        /// <summary>
        /// Extracts version number from user agent string.
        /// </summary>
        private string ExtractVersion(string userAgent, string browserName)
        {
            try
            {
                var index = userAgent.IndexOf(browserName, StringComparison.OrdinalIgnoreCase);
                if (index < 0) return "Unknown";

                var versionString = userAgent.Substring(index + browserName.Length).TrimStart('/', ' ');
                var version = versionString.Split(new[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                return version ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Encodes password to Base64 format (as expected by stored procedure).
        /// </summary>
        private string EncodePasswordToBase64(string plainPassword)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainPassword);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch
            {
                return plainPassword;
            }
        }
    }
}
