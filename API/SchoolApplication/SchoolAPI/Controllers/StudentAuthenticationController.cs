using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities.Authentication;
using System.Text;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing student authentication.
    /// </summary>
    [AllowAnonymous]
    public class StudentAuthenticationController : BaseApiController
    {
        private readonly IStudentAuthenticationService _studentAuthenticationService;

        public StudentAuthenticationController(
            IStudentAuthenticationService studentAuthenticationService,
            ILogger<StudentAuthenticationController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _studentAuthenticationService = studentAuthenticationService ?? throw new ArgumentNullException(nameof(studentAuthenticationService));
        }

        /// <summary>
        /// Authenticate student with roll number and password
        /// </summary>
        /// <param name="request">Student login request containing roll number and password</param>
        /// <returns>Student authentication result with profile information</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginStudent([FromBody] StudentLoginRequest request)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for StudentAuthentication. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                // Get client information from request headers
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var sessionId = Guid.NewGuid().ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var (browser, browserVersion) = ExtractBrowserInfo();

                // IMPORTANT: Encode password EXACTLY as your stored procedure expects
                // The stored procedure expects the password in a specific format
                // Remove any extra encoding - just encode once to Base64
                var encodedPassword = EncodePasswordToBase64(request.StudPassword);

                _logger.LogInformation("Password encoding debug - Original: {Original}, Encoded: {Encoded}", 
                    request.StudPassword, encodedPassword);

                // Authenticate student with encoded password
                var authResult = await _studentAuthenticationService.LoginStudentAsync(
                    request.RollNo,
                    encodedPassword,
                    ipAddress,
                    sessionId,
                    browser,
                    browserVersion,
                    userAgent
                    //request.DeviceId
                );

                if (authResult.StatusCode == 200)
                {
                    _logger.LogInformation("User logged in successfully. LoginCode: {LoginCode}", request.RollNo);
                    return Ok(authResult);
                }

                return StatusCode(authResult.StatusCode, authResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user: {LoginCode}. Exception: {Message}", 
                    request.RollNo, ex.Message);
                return ErrorResponse("An unexpected error occurred during authentication", 500, "AUTH_ERROR");
            }
        }
        
        /// <summary>
        /// Logout student
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutStudent([FromBody] StudentLogoutRequest request)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for StudentLogout. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _studentAuthenticationService.LogoutStudentAsync(request.StudentId, request.SessionId);

                _logger.LogInformation("Student logged out successfully. StudentId: {StudentId}", request.StudentId);

                return SuccessResponse(result, "Student logout successful.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during student logout for StudentId: {StudentId}", request?.StudentId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Encodes password to Base64 format (as expected by stored procedure).
        /// </summary>
        private string EncodePasswordToBase64(string plainPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainPassword))
                    return plainPassword;

                // IMPORTANT: Determine the correct encryption method
                // Your working screen uses: 28/06/2006 -> TWpndk1EWXZNakF3Tmc9PQ==
                // This suggests a different encoding than simple Base64
                
                // Option A: If it's double Base64 encoding
                var firstEncode = Encoding.UTF8.GetBytes(plainPassword);
                var firstBase64 = Convert.ToBase64String(firstEncode);
                var secondEncode = Encoding.UTF8.GetBytes(firstBase64);
                var secondBase64 = Convert.ToBase64String(secondEncode);
                
                _logger.LogInformation("Password debug - Plain: {Plain}, FirstBase64: {First}, SecondBase64: {Second}", 
                    plainPassword, firstBase64, secondBase64);
                
                // Return based on what matches your database
                return secondBase64;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error encoding password");
                return plainPassword;
            }
        }

        /// <summary>
        /// Extracts browser name and version from user agent.
        /// </summary>
        private (string browser, string version) ExtractBrowserInfo()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(userAgent))
                return ("Unknown", "Unknown");

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
    }
}
