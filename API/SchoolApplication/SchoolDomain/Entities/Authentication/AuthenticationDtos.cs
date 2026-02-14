using System.ComponentModel.DataAnnotations;

namespace SchoolDomain.Entities.Authentication
{
    /// <summary>
    /// Login request DTO for user authentication.
    /// Minimal request with only required fields: loginCode and userPassword.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// User's login code (e.g., 'senthil')
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Login code is required")]
        public string LoginCode { get; set; } = string.Empty;

        /// <summary>
        /// User's password (plaintext - will be encoded to Base64)
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Login response DTO containing user and session details.
    /// </summary>
    public class LoginResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public LoginDataDto? Data { get; set; }
        public string? ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Contains authenticated user details and permissions.
    /// </summary>
    public class LoginDataDto
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
        
        /// <summary>
        /// JWT access token for API authentication
        /// </summary>
        public string? AccessToken { get; set; }
        
        /// <summary>
        /// Token refresh token
        /// </summary>
        public string? RefreshToken { get; set; }
        
        /// <summary>
        /// Token expiration time
        /// </summary>
        public DateTime? TokenExpiryTime { get; set; }

        public List<ModulePermissionDto> PermittedModules { get; set; } = new();
        public List<ModuleNotPermissionDto> NotPermittedModules { get; set; } = new();
    }

    /// <summary>
    /// Module permission details for user.
    /// </summary>
    public class ModulePermissionDto
    {
        /// <summary>
        /// Module name
        /// </summary>
        public string fn { get; set; } = string.Empty;

        /// <summary>
        /// Module ID
        /// </summary>
        public int mid { get; set; }

        /// <summary>
        /// Assignment status (0=Not assigned, 1=Assigned, 2=Denied)
        /// </summary>
        public int st { get; set; }

        /// <summary>
        /// Is module active
        /// </summary>
        public int act { get; set; }
    }
    public class ModuleNotPermissionDto
    {
        /// <summary>
        /// Module name
        /// </summary>
        public string mn { get; set; } = string.Empty;

        /// <summary>
        /// Module ID
        /// </summary>
        public int mid { get; set; }
    }
    /// <summary>
    /// User profile information.
    /// </summary>
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string LoginCode { get; set; } = string.Empty;
        public string WelcomeName { get; set; } = string.Empty;
        public int StaffId { get; set; }
        public string Designation { get; set; } = string.Empty;
        public string CharitiesName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }

    /// <summary>
    /// Logout request DTO.
    /// </summary>
    public class LogoutRequestDto
    {
        public int UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Refresh token request DTO.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Login result codes from stored procedure.
    /// </summary>
    public enum LoginResultCode
    {
        InvalidLoginCode = 0,
        LoginSuccess = 1,
        InvalidPasswordOrUsername = 2,
        NoPermissionToModule = 3,
        AccountLocked = 4
    }
}
