using SchoolDomain.Entities.Authentication;

namespace SchoolApplication.Interface
{
    /// <summary>
    /// Service for authentication business logic including token generation.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates user and generates JWT tokens.
        /// </summary>
        /// <param name="loginCode">User login code</param>
        /// <param name="userPassword">Encrypted password</param>
        /// <param name="moduleName">Optional module name</param>
        /// <param name="userAuthCode">Optional auth code</param>
        /// <param name="ipAddress">Client IP address</param>
        /// <param name="sessionId">Session ID</param>
        /// <param name="browser">Browser name</param>
        /// <param name="browserVersion">Browser version</param>
        /// <param name="userAgent">User agent string</param>
        Task<LoginResponseDto> LoginAsync(
            string loginCode,
            string userPassword,
            string? moduleName,
            string? userAuthCode,
            string ipAddress,
            string sessionId,
            string browser,
            string browserVersion,
            string userAgent);

        /// <summary>
        /// Logout user and invalidate session.
        /// </summary>
        Task<bool> LogoutAsync(int userId, string sessionId);

        /// <summary>
        /// Refresh access token using refresh token.
        /// </summary>
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Get user profile information.
        /// </summary>
        Task<UserProfileDto?> GetUserProfileAsync(int userId);
    }

    /// <summary>
    /// Token response DTO.
    /// </summary>
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public int ExpiresIn { get; set; }
    }
}
