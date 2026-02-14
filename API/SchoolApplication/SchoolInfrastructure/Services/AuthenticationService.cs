using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolApplication.Interface;
using SchoolDomain.Entities.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolInfrastructure.Services
{
    /// <summary>
    /// Service for authentication and JWT token management.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authRepository;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpiryMinutes;
        private readonly int _refreshTokenExpiryDays;

        public AuthenticationService(
            IAuthenticationRepository authRepository,
            ILogger<AuthenticationService> logger,
            IConfiguration configuration)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var jwtSettings = _configuration.GetSection("JwtSettings");
            _jwtSecret = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
            _jwtIssuer = jwtSettings["Issuer"] ?? "SchoolERP";
            _jwtAudience = jwtSettings["Audience"] ?? "SchoolERPClients";
            _jwtExpiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var expiryMinutes) ? expiryMinutes : 60;
            _refreshTokenExpiryDays = int.TryParse(jwtSettings["RefreshTokenExpiryDays"], out var refreshExpiryDays) ? refreshExpiryDays : 7;
        }

        public async Task<LoginResponseDto> LoginAsync(
            string loginCode,
            string userPassword,
            string? moduleName,
            string? userAuthCode,
            string ipAddress,
            string sessionId,
            string browser,
            string browserVersion,
            string userAgent)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(loginCode);
            ArgumentException.ThrowIfNullOrWhiteSpace(userPassword);
            ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
            ArgumentException.ThrowIfNullOrWhiteSpace(browser);
            ArgumentException.ThrowIfNullOrWhiteSpace(browserVersion);

            try
            {
                // Call authentication repository
                var authResult = await _authRepository.AuthenticateUserAsync(
                    loginCode,
                    userPassword,
                    moduleName,
                    userAuthCode,
                    ipAddress,
                    sessionId,
                    browser,
                    browserVersion,
                    userAgent
                );

                // Handle authentication result codes
                return HandleAuthenticationResult(authResult, sessionId);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument during login for user: {LoginCode}", loginCode);
                return new LoginResponseDto
                {
                    StatusCode = 400,
                    Message = "Invalid request parameters",
                    ErrorCode = "INVALID_REQUEST",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user: {LoginCode}. Exception: {ExceptionMessage}", loginCode, ex.Message);
                return new LoginResponseDto
                {
                    StatusCode = 500,
                    Message = "An error occurred during authentication",
                    ErrorCode = "AUTH_ERROR",
                    Data = null
                };
            }
        }

        public async Task<bool> LogoutAsync(int userId, string sessionId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0", nameof(userId));
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

            try
            {
                var result = await _authRepository.LogoutUserAsync(userId, sessionId);
                _logger.LogInformation("User logged out. UserId: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

            try
            {
                // Validate refresh token
                var principal = ValidateRefreshToken(refreshToken);
                if (principal == null)
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

                // Extract user claims
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                var loginCodeClaim = principal.FindFirst("loginCode");

                if (userIdClaim == null || loginCodeClaim == null)
                {
                    throw new SecurityTokenException("Invalid token claims");
                }

                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    throw new SecurityTokenException("Invalid UserId in token");
                }

                // Generate new access token
                var accessToken = GenerateAccessToken(userId, loginCodeClaim.Value);
                var newRefreshToken = GenerateRefreshToken(userId, loginCodeClaim.Value);

                _logger.LogInformation("Token refreshed for UserId: {UserId}", userId);

                return new TokenResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes),
                    ExpiresIn = _jwtExpiryMinutes * 60
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                throw;
            }
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0", nameof(userId));

            try
            {
                return await _authRepository.GetUserProfileAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user profile for UserId: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Handles authentication result codes and creates appropriate response.
        /// </summary>
        private LoginResponseDto HandleAuthenticationResult(LoginAuthenticationResult authResult, string sessionId)
        {
            var resultCode = (LoginResultCode)authResult.ResultCode;

            _logger.LogInformation("Authentication result code: {ResultCode} for user: {LoginCode}", resultCode, authResult.LoginCode);

            return resultCode switch
            {
                LoginResultCode.LoginSuccess => HandleLoginSuccess(authResult, sessionId),
                LoginResultCode.InvalidLoginCode => new LoginResponseDto
                {
                    StatusCode = 401,
                    Message = "Invalid login code",
                    ErrorCode = "INVALID_LOGIN_CODE",
                    Data = null
                },
                LoginResultCode.InvalidPasswordOrUsername => new LoginResponseDto
                {
                    StatusCode = 401,
                    Message = "Invalid username or password",
                    ErrorCode = "INVALID_CREDENTIALS",
                    Data = null
                },
                LoginResultCode.NoPermissionToModule => new LoginResponseDto
                {
                    StatusCode = 403,
                    Message = "No permission to access the specified module",
                    ErrorCode = "NO_MODULE_PERMISSION",
                    Data = null
                },
                LoginResultCode.AccountLocked => new LoginResponseDto
                {
                    StatusCode = 403,
                    Message = "Account is locked due to multiple failed login attempts",
                    ErrorCode = "ACCOUNT_LOCKED",
                    Data = null
                },
                _ => new LoginResponseDto
                {
                    StatusCode = 500,
                    Message = "Unknown authentication error",
                    ErrorCode = "UNKNOWN_ERROR",
                    Data = null
                }
            };
        }

        /// <summary>
        /// Handles successful login and generates tokens.
        /// </summary>
        private LoginResponseDto HandleLoginSuccess(LoginAuthenticationResult authResult, string sessionId)
        {
            try
            {
                var accessToken = GenerateAccessToken(authResult.UserId, authResult.LoginCode);
                var refreshToken = GenerateRefreshToken(authResult.UserId, authResult.LoginCode);
                var expiryTime = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes);

                var loginData = new LoginDataDto
                {
                    UserId = authResult.UserId,
                    LoginCode = authResult.LoginCode,
                    WelcomeName = authResult.WelcomeName,
                    lastLogin = authResult.lastLogin,
                    lloginid = authResult.lloginid,
                    modId = authResult.modId,
                    sessionId = sessionId,
                    img = authResult.img,
                    imgPath = authResult.imgPath,
                    StaffId = authResult.StaffId,
                    designation = authResult.designation,
                    charitiesName = authResult.charitiesName,
                    coeVer = authResult.coeVer,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiryTime = expiryTime
                };

                _logger.LogInformation("Login successful for user: {LoginCode} (UserId: {UserId})", authResult.LoginCode, authResult.UserId);

                return new LoginResponseDto
                {
                    StatusCode = 200,
                    Message = "Login successful",
                    Data = loginData,
                    ErrorCode = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating tokens for user: {LoginCode}", authResult.LoginCode);
                throw;
            }
        }

        /// <summary>
        /// Generates JWT access token.
        /// </summary>
        private string GenerateAccessToken(int userId, string loginCode)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("loginCode", loginCode),
                new Claim(ClaimTypes.Name, loginCode),
                new Claim("tokenType", "access")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates JWT refresh token with longer expiry.
        /// </summary>
        private string GenerateRefreshToken(int userId, string loginCode)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("loginCode", loginCode),
                new Claim("tokenType", "refresh")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Validates and reads refresh token.
        /// </summary>
        private ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);

                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Check if token type is refresh
                var tokenTypeClaim = principal.FindFirst("tokenType");
                if (tokenTypeClaim?.Value != "refresh")
                {
                    return null;
                }

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error validating refresh token");
                return null;
            }
        }

        /// <summary>
        /// Generates a unique session ID.
        /// </summary>
        public static string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 25);
        }
    }
}
