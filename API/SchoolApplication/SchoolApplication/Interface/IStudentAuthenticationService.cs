using SchoolDomain.Entities.Authentication;

namespace SchoolApplication.Interface
{
    /// <summary>
    /// Interface for Student Authentication Service
    /// </summary>
    public interface IStudentAuthenticationService
    {
        /// <summary>
        /// Authenticate student with login credentials
        /// </summary>
        Task<StudentLoginResponseDto> LoginStudentAsync(
            string RollNo,
            string StudPassword,
            string ipAddress,
            string sessionId,
            string browser,
            string browserVersion,
            string userAgent,
            string? deviceId = null
        );

        /// <summary>
        /// Logout student
        /// </summary>
        Task<bool> LogoutStudentAsync(int studentId, string sessionId);
    }
}
