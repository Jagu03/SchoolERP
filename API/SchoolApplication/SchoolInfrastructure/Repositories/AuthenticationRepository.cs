using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities.Authentication;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    /// <summary>
    /// Repository for authentication operations calling stored procedures.
    /// </summary>
    public class AuthenticationRepository : BaseRepository, IAuthenticationRepository
    {
        public AuthenticationRepository(IConfiguration configuration, ILogger<AuthenticationRepository> logger)
            : base(configuration, logger)
        {
        }

        public async Task<LoginAuthenticationResult> AuthenticateUserAsync(
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
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add("@LoginCode", loginCode, DbType.String, size: 30);
                parameters.Add("@UserPassword", userPassword, DbType.String, size: 100);
                // Handle moduleName - convert string "NULL" to actual null value
                var moduleNameValue = string.IsNullOrWhiteSpace(moduleName) || moduleName.Equals("NULL", StringComparison.OrdinalIgnoreCase) 
                    ? (object)DBNull.Value 
                    : moduleName;
                parameters.Add("@ModuleName", moduleNameValue, DbType.String, size: 100);
                parameters.Add("@userAuthCode", userAuthCode ?? string.Empty, DbType.String, size: 300);
                parameters.Add("@IPAddr", ipAddress, DbType.String, size: 50);
                parameters.Add("@SessId", sessionId, DbType.String, size: 200);
                parameters.Add("@Browser", browser, DbType.String, size: 200);
                parameters.Add("@BVersion", browserVersion, DbType.String, size: 20);
                parameters.Add("@PCUSName", userAgent ?? string.Empty, DbType.String, size: 250);
                parameters.Add("@result", dbType: DbType.Byte, direction: ParameterDirection.Output);

                // Open connection explicitly before executing
                await connection.OpenAsync();

                // Execute the stored procedure and get the result set
                // We need to use QueryFirstOrDefaultAsync which properly handles output parameters
                dynamic? userData = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "[HR].[AuthenticateUserCommonLogin]",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 30);

                // Get the result code from output parameter
                // The output parameter is set by the stored procedure
                byte? resultCodeNullable = parameters.Get<byte?>("@result");
                byte resultCode = resultCodeNullable ?? 0;
                bool hasUserData = userData != null;

                // Log the result code
                Logger.LogInformation("Stored procedure execution for login code: {LoginCode}, result code: {ResultCode}, user data: {HasUserData}", 
                    loginCode, resultCode, hasUserData);

                // If result code is not success (1), return early without processing data
                if (resultCode != 1)
                {
                    Logger.LogWarning("Authentication procedure returned non-success code {ResultCode} for login code: {LoginCode}, has user data: {HasUserData}", 
                        resultCode, loginCode, hasUserData);
                    return new LoginAuthenticationResult
                    {
                        ResultCode = resultCode,
                        LoginCode = loginCode
                    };
                }

                // Check if user data was returned from stored procedure
                if (userData == null)
                {
                    Logger.LogWarning("Authentication failed for login code: {LoginCode}, result code: {ResultCode} - No user data returned", 
                        loginCode, resultCode);
                    return new LoginAuthenticationResult
                    {
                        ResultCode = resultCode,
                        LoginCode = loginCode
                    };
                }

                var result = new LoginAuthenticationResult
                {
                    UserId = userData.UserId,
                    LoginCode = userData.LoginCode,
                    WelcomeName = userData.WelcomeName,
                    lastLogin = userData.lastLogin ?? "This is first login.",
                    lloginid = userData.lloginid ?? 0,
                    modId = userData.modId,
                    sessionId = userData.sessionId,
                    img = userData.img,
                    imgPath = userData.imgPath,
                    StaffId = userData.StaffId ?? 0,
                    designation = userData.designation ?? "-",
                    charitiesName = userData.charitiesName ?? "-",
                    coeVer = userData.coeVer ?? 1,
                    ResultCode = resultCode
                };

                Logger.LogInformation("User authenticated successfully. UserId: {UserId}, LoginCode: {LoginCode}", 
                    result.UserId, result.LoginCode);

                return result;
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex, "SQL Error during authentication for LoginCode: {LoginCode}. SqlError: {SqlErrorMessage}", loginCode, ex.Message);
                throw HandleDatabaseError(ex, "AuthenticateUserAsync", $"LoginCode={loginCode}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error during authentication for LoginCode: {LoginCode}. Exception: {ExceptionMessage}", loginCode, ex.Message);
                LogUnexpectedError(ex, "AuthenticateUserAsync", $"LoginCode={loginCode}");
                throw;
            }
        }

        public async Task<IEnumerable<ModulePermissionDto>> GetPermittedModulesAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0", nameof(userId));

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int16);

                var modules = await connection.QueryAsync<ModulePermissionDto>(
                    "[HR].[FetchAllPermittedModulesForUser]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                Logger.LogInformation("Fetched {Count} permitted modules for UserId: {UserId}", modules.Count(), userId);
                return modules;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "GetPermittedModulesAsync", $"UserId={userId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "GetPermittedModulesAsync", $"UserId={userId}");
                throw;
            }
        }

        public async Task<IEnumerable<ModuleNotPermissionDto>> GetNotPermittedModulesAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0", nameof(userId));

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int16);

                var modules = await connection.QueryAsync<ModuleNotPermissionDto>(
                    "[HR].[FetchNotPermittedModulesForUser]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                Logger.LogInformation("Fetched {Count} not-permitted modules for UserId: {UserId}", modules.Count(), userId);
                return modules;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "GetNotPermittedModulesAsync", $"UserId={userId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "GetNotPermittedModulesAsync", $"UserId={userId}");
                throw;
            }
        }

        public async Task<bool> LogoutUserAsync(int userId, string sessionId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0", nameof(userId));
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

            try
            {
                await using var connection = CreateConnection();
                
                // Update LoginAudit table to mark logout
                var query = @"
                    UPDATE ILogs.LoginAudit 
                    SET LogoutTime = GETUTCDATE() 
                    WHERE UserId = @UserId AND GenSessionId = @SessionId AND LogoutTime IS NULL";

                var result = await connection.ExecuteAsync(query, new { UserId = userId, SessionId = sessionId });

                Logger.LogInformation("User logged out successfully. UserId: {UserId}, SessionId: {SessionId}", userId, sessionId);
                return result > 0;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "LogoutUserAsync", $"UserId={userId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "LogoutUserAsync", $"UserId={userId}");
                throw;
            }
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be greater than 0", nameof(userId));

            try
            {
                await using var connection = CreateConnection();
                var query = @"
                    SELECT 
                        a.UserId, 
                        a.LoginCode, 
                        a.WelcomeName,
                        ISNULL(us.StaffId, 0) AS StaffId,
                        ISNULL(fsi.ds, '-') AS Designation,
                        ISNULL(c.FullName, '-') AS CharitiesName,
                        ISNULL(ui.GenFName, 'default.png') AS Image,
                        CASE WHEN ui.GenFName IS NOT NULL THEN 'Resx/UserImages/' 
                             ELSE 'Resx/StaffImages/' END AS ImagePath
                    FROM HR.Users a 
                    LEFT JOIN HR.UserImages ui ON a.UserId = ui.UserId AND ISNULL(ui.IsCur, 1) = 1
                    LEFT JOIN HR.UserStaffs us ON us.userId = a.UserId
                    LEFT JOIN Staff.FetchStaffInfoCompact() fsi ON fsi.Sid = us.StaffId
                    LEFT JOIN Data.Charities c ON 1=1
                    WHERE a.UserId = @UserId";

                var profile = await connection.QueryFirstOrDefaultAsync<UserProfileDto>(query, 
                    new { UserId = userId });

                if (profile != null)
                    Logger.LogInformation("User profile fetched. UserId: {UserId}", userId);

                return profile;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "GetUserProfileAsync", $"UserId={userId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "GetUserProfileAsync", $"UserId={userId}");
                throw;
            }
        }
    }
}
