using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities.Authentication;
using System.Data;
using System.Text;

namespace SchoolInfrastructure.Repositories
{
    /// <summary>
    /// Repository for student authentication operations calling stored procedures.
    /// </summary>
    public class StudentAuthenticationRepository : BaseRepository, IStudentAuthenticationRepository
    {
        public StudentAuthenticationRepository(IConfiguration configuration, ILogger<StudentAuthenticationRepository> logger)
            : base(configuration, logger)
        {
        }

        public async Task<StudentAuthenticationResult> AuthenticateStudentAsync(
            string rollNo,
            string studPassword,
            string? dob,
            byte loginAs,
            string ipAddress,
            string sessionId,
            string browser,
            string browserVersion,
            string userAgent,
            string? deviceId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(rollNo);
            ArgumentException.ThrowIfNullOrWhiteSpace(studPassword);
            ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
            ArgumentException.ThrowIfNullOrWhiteSpace(browser);
            ArgumentException.ThrowIfNullOrWhiteSpace(browserVersion);

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();

                // Add parameters in the exact order and names expected by the stored procedure
                parameters.Add("@RollNo", rollNo, DbType.String, size: 100);
                parameters.Add("@StudPassword", studPassword, DbType.String, size: 200);
                parameters.Add("@dob", string.IsNullOrWhiteSpace(dob) ? DBNull.Value : dob, DbType.String, size: 10);
                parameters.Add("@loginAs", loginAs, DbType.Byte);
                parameters.Add("@IPAddr", ipAddress, DbType.String, size: 50);
                parameters.Add("@SessId", sessionId, DbType.String, size: 200);
                parameters.Add("@Browser", browser, DbType.String, size: 50);
                parameters.Add("@BVersion", browserVersion, DbType.String, size: 20);
                parameters.Add("@deviceId", string.IsNullOrWhiteSpace(deviceId) ? DBNull.Value : deviceId, DbType.String, size: 2000);
                
                // Add output parameter - Dapper will populate this after execution
                parameters.Add("@result", dbType: DbType.Byte, direction: ParameterDirection.Output);

                // Execute the stored procedure
                await connection.ExecuteAsync(
                    "[HR].[AuthenticateStudent]",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 30);

                // Get the result code from output parameter
                byte resultCode = parameters.Get<byte>("@result");

                Logger.LogInformation("AuthenticateStudent returned result code: {ResultCode} for RollNo: {RollNo}", 
                    resultCode, rollNo);

                // If result code is not success (1), return early without processing data
                if (resultCode != 1)
                {
                    Logger.LogWarning("Student authentication returned non-success code {ResultCode} for roll number: {RollNo}", 
                        resultCode, rollNo);
                    return new StudentAuthenticationResult
                    {
                        ResultCode = resultCode,
                        RollNo = rollNo
                    };
                }

                // For successful authentication, the stored procedure should return a result set
                // Re-execute to get the student data since ExecuteAsync doesn't return results
                await using var connectionForData = CreateConnection();
                var studentData = await connectionForData.QueryFirstOrDefaultAsync<dynamic>(
                    "[HR].[AuthenticateStudent]",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 30);

                // Check if student data was returned from stored procedure
                if (studentData == null)
                {
                    Logger.LogWarning("Student authentication failed for roll number: {RollNo}, result code: {ResultCode} - No student data returned", 
                        rollNo, resultCode);
                    return new StudentAuthenticationResult
                    {
                        ResultCode = resultCode,
                        RollNo = rollNo
                    };
                }

                var result = new StudentAuthenticationResult
                {
                    LoginId = studentData.LoginId,
                    LastLoginTime = studentData.LastLoginTime ?? "This is first login.",
                    StudentId = studentData.StudentId,
                    StudentName = studentData.StudentName ?? string.Empty,
                    RollNo = studentData.RollNo ?? rollNo,
                    ImageName = studentData.ImageName ?? "00000000-0000-0000-0000-000000000000.png",
                    RegisterNumber = studentData.RegisterNumber ?? string.Empty,
                    ImpresCode = studentData.ImpresCode ?? string.Empty,
                    Quota = studentData.Quota ?? string.Empty,
                    Community = studentData.Community ?? string.Empty,
                    StudType = studentData.StudType ?? string.Empty,
                    FirstGrad = studentData.FirstGrad ?? string.Empty,
                    HostelerId = studentData.HostelerId ?? 0,
                    Gender = studentData.Gender ?? string.Empty,
                    ShortGender = studentData.ShortGender ?? string.Empty,
                    StudStat = studentData.StudStat ?? 0,
                    ReqHostel = studentData.ReqHostel ?? 0,
                    ReqTransport = studentData.ReqTransport ?? 0,
                    InstId = studentData.InstId ?? 0,
                    SType = studentData.SType ?? string.Empty,
                    AdmnTypeShort = studentData.AdmnTypeShort ?? string.Empty,
                    AdmnType = studentData.AdmnType ?? string.Empty,
                    IsClassRep = SafeConvertToByte(studentData.icr),
                    RepSl = studentData.rsl ?? 0,
                    LoginAs = studentData.loginAs ?? loginAs,
                    ProfilePic = studentData.pic ?? string.Empty,
                    ResultCode = resultCode
                };

                Logger.LogInformation("Student authenticated successfully. StudentId: {StudentId}, RollNo: {RollNo}", 
                    result.StudentId, result.RollNo);

                return result;
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex, "SQL Error during student authentication for RollNo: {RollNo}. SqlError: {SqlErrorMessage}, SqlState: {SqlState}, LineNumber: {LineNumber}", 
                    rollNo, ex.Message, ex.State, ex.LineNumber);
                throw HandleDatabaseError(ex, "AuthenticateStudentAsync", $"RollNo={rollNo}");
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex, "Invalid operation during student authentication for RollNo: {RollNo}. Details: {ExceptionMessage}", rollNo, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error during student authentication for RollNo: {RollNo}. Exception Type: {ExceptionType}, Message: {ExceptionMessage}", 
                    rollNo, ex.GetType().Name, ex.Message);
                LogUnexpectedError(ex, "AuthenticateStudentAsync", $"RollNo={rollNo}");
                throw;
            }
        }

        /// <summary>
        /// Safely converts a value to byte, returning 0 if conversion fails.
        /// </summary>
        private static byte SafeConvertToByte(object? value)
        {
            if (value == null)
                return 0;

            if (value is byte b)
                return b;

            if (byte.TryParse(value.ToString() ?? "0", out byte result))
                return result;

            return 0;
        }
    }
}

