using SchoolDomain.Entities.Authentication;

namespace SchoolApplication.Interface
{
    /// <summary>
    /// Interface for Student Authentication Repository
    /// </summary>
    public interface IStudentAuthenticationRepository
    {
        /// <summary>
        /// Authenticate student with roll number and password
        /// </summary>
        Task<StudentAuthenticationResult> AuthenticateStudentAsync(
            string rollNo,
            string studPassword,
            string? dob,
            byte loginAs,
            string ipAddress,
            string sessionId,
            string browser,
            string browserVersion,
            string userAgent,
            string? deviceId = null
        );
    }
}
