using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace SchoolInfrastructure.Services
{
    /// <summary>
    /// Service for student authentication operations
    /// </summary>
    public class StudentAuthenticationService : IStudentAuthenticationService
    {
        private readonly IStudentAuthenticationRepository _studentAuthenticationRepository;
        private readonly ILogger<StudentAuthenticationService> _logger;
        private readonly IConfiguration _configuration;

        public StudentAuthenticationService(
            IStudentAuthenticationRepository studentAuthenticationRepository,
            ILogger<StudentAuthenticationService> logger,
            IConfiguration configuration)
        {
            _studentAuthenticationRepository = studentAuthenticationRepository ?? throw new ArgumentNullException(nameof(studentAuthenticationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<StudentLoginResponseDto> LoginStudentAsync(
            string rollNo,
            string studPassword,
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
                // Call repository to authenticate student
                var authResult = await _studentAuthenticationRepository.AuthenticateStudentAsync(
                    rollNo,
                    studPassword,
                    null,
                    1,
                    ipAddress,
                    sessionId,
                    browser,
                    browserVersion,
                    userAgent,
                    deviceId
                );

                if (authResult.ResultCode != 1)
                {
                    _logger.LogWarning("Student authentication failed. RollNo: {RollNo}, ResultCode: {ResultCode}", 
                        rollNo, authResult.ResultCode);
                    return new StudentLoginResponseDto
                    {
                        StatusCode = 401,
                        Message = "Invalid credentials",
                        Data = null,
                        ErrorCode = "INVALID_CREDENTIALS"
                    };
                }

                _logger.LogInformation("Student logged in successfully. StudentId: {StudentId}, RollNo: {RollNo}", 
                    authResult.StudentId, authResult.RollNo);

                var loginData = MapAuthenticationResultToLoginData(authResult);
                return new StudentLoginResponseDto
                {
                    StatusCode = 200,
                    Message = "Login successful",
                    Data = loginData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during student login for RollNo: {RollNo}", rollNo);
                throw;
            }
        }

        /// <summary>
        /// Maps StudentAuthenticationResult to StudentLoginDataDto
        /// </summary>
        private static StudentLoginDataDto MapAuthenticationResultToLoginData(StudentAuthenticationResult authResult)
        {
            return new StudentLoginDataDto
            {
                LoginId = authResult.LoginId,
                LastLoginTime = authResult.LastLoginTime,
                StudentId = authResult.StudentId,
                StudentName = authResult.StudentName,
                ImageName = authResult.ImageName,
                RegisterNumber = authResult.RegisterNumber,
                ImpresCode = authResult.ImpresCode,
                Quota = authResult.Quota,
                Community = authResult.Community,
                StudType = authResult.StudType,
                FirstGrad = authResult.FirstGrad,
                HostelerId = authResult.HostelerId,
                Gender = authResult.Gender,
                ShortGender = authResult.ShortGender,
                StudStat = authResult.StudStat,
                ReqHostel = authResult.ReqHostel,
                ReqTransport = authResult.ReqTransport,
                InstId = authResult.InstId,
                SType = authResult.SType,
                AdmnTypeShort = authResult.AdmnTypeShort,
                AdmnType = authResult.AdmnType,
                IsClassRep = authResult.IsClassRep,
                RepSl = authResult.RepSl,
                LoginAs = authResult.LoginAs,
                ProfilePic = authResult.ProfilePic,
                RollNo = authResult.RollNo
            };
        }

        public async Task<bool> LogoutStudentAsync(int studentId, string sessionId)
        {
            if (studentId <= 0)
                throw new ArgumentException("StudentId must be greater than 0", nameof(studentId));

            ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

            try
            {
                // Update student login audit table to mark logout
                // This can be implemented when audit logging service is created
                _logger.LogInformation("Student logged out. StudentId: {StudentId}, SessionId: {SessionId}", studentId, sessionId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during student logout for StudentId: {StudentId}", studentId);
                throw;
            }
        }
    }
}
