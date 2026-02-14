namespace SchoolAPI.Exceptions
{
    public class SchoolApiException : Exception
    {
        public int StatusCode { get; set; }
        public string? ErrorCode { get; set; }

        public SchoolApiException(string message, int statusCode = 500, string? errorCode = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        public SchoolApiException(string message, Exception innerException, int statusCode = 500, string? errorCode = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }

    public class ValidationException : SchoolApiException
    {
        public ValidationException(string message, string? errorCode = "VALIDATION_ERROR")
            : base(message, 400, errorCode)
        {
        }
    }

    public class NotFoundException : SchoolApiException
    {
        public NotFoundException(string message, string? errorCode = "NOT_FOUND")
            : base(message, 404, errorCode)
        {
        }
    }

    public class UnauthorizedException : SchoolApiException
    {
        public UnauthorizedException(string message = "Unauthorized access.", string? errorCode = "UNAUTHORIZED")
            : base(message, 401, errorCode)
        {
        }
    }

    public class ForbiddenException : SchoolApiException
    {
        public ForbiddenException(string message = "Access forbidden.", string? errorCode = "FORBIDDEN")
            : base(message, 403, errorCode)
        {
        }
    }

    public class BusinessLogicException : SchoolApiException
    {
        public BusinessLogicException(string message, string? errorCode = "BUSINESS_LOGIC_ERROR")
            : base(message, 500, errorCode)
        {
        }

        public BusinessLogicException(string message, Exception innerException, string? errorCode = "BUSINESS_LOGIC_ERROR")
            : base(message, innerException, 500, errorCode)
        {
        }
    }
}
