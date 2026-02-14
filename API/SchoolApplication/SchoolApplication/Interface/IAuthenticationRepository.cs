using SchoolDomain.Entities.Authentication;

namespace SchoolApplication.Interface
{
    /// <summary>
    /// Interface for authentication-related database operations.
    /// </summary>
    public interface IAuthenticationRepository
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="loginCode">User's login code</param>
        /// <param name="userPassword">User's encrypted password</param>
        /// <param name="moduleName">Optional module name for permission check</param>
        /// <param name="userAuthCode">Optional authentication code</param>
        /// <param name="ipAddress">Client's IP address</param>
        /// <param name="sessionId">Generated session ID</param>
        /// <param name="browser">Browser name</param>
        /// <param name="browserVersion">Browser version</param>
        /// <param name="userAgent">User agent string</param>
        /// <returns>Login response with user details and permissions</returns>
        Task<LoginAuthenticationResult> AuthenticateUserAsync(
            string loginCode,
            string userPassword,
            string? moduleName,
            string? userAuthCode,
            string ipAddress,
            string sessionId,
            string browser,
            string browserVersion,
            string userAgent
        );

        /// <summary>
        /// Retrieves permitted modules for a user.
        /// </summary>
        Task<IEnumerable<ModulePermissionDto>> GetPermittedModulesAsync(int userId);

        /// <summary>
        /// Retrieves non-permitted modules for a user.
        /// </summary>
        Task<IEnumerable<ModuleNotPermissionDto>> GetNotPermittedModulesAsync(int userId);

        /// <summary>
        /// Records user logout in audit table.
        /// </summary>
        Task<bool> LogoutUserAsync(int userId, string sessionId);

        /// <summary>
        /// Gets user profile information.
        /// </summary>
        Task<UserProfileDto?> GetUserProfileAsync(int userId);
    }

    /// <summary>
    /// Result object from authentication procedure.
    /// </summary>
    public class LoginAuthenticationResult
    {
        public int UserId { get; set; }
        public string LoginCode { get; set; } = string.Empty;
        public string WelcomeName { get; set; } = string.Empty;
        public string lastLogin { get; set; } = string.Empty;
        public long lloginid { get; set; }
        public int? modId { get; set; }
        public string sessionId { get; set; } = string.Empty;
        public string img { get; set; } = string.Empty;
        public string imgPath { get; set; } = string.Empty;
        public int StaffId { get; set; }
        public string designation { get; set; } = string.Empty;
        public string charitiesName { get; set; } = string.Empty;
        public int coeVer { get; set; }
        public int ResultCode { get; set; }
    }
}
