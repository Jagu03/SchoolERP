namespace SchoolDomain.Entities.Authentication
{
    /// <summary>
    /// DTO for Student Login Request
    /// </summary>
    public class StudentLoginRequest
    {
        public string RollNo { get; set; } = string.Empty;
        public string StudPassword { get; set; } = string.Empty;
        //public string? DOB { get; set; }
        //public byte LoginAs { get; set; } = 1; // 1 = Student, 0 = Parent
        //public string? DeviceId { get; set; }
    }

    /// <summary>
    /// DTO for Student Logout Request
    /// </summary>
    public class StudentLogoutRequest
    {
        public int StudentId { get; set; }
        public string SessionId { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for Student Authentication Result
    /// </summary>
    public class StudentAuthenticationResult
    {
        public byte ResultCode { get; set; } // 0 = Invalid, 1 = Success, 2 = Not Found
        public string RollNo { get; set; } = string.Empty;
        public long LoginId { get; set; }
        public string LastLoginTime { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string RegisterNumber { get; set; } = string.Empty;
        public string ImpresCode { get; set; } = string.Empty;
        public string Quota { get; set; } = string.Empty;
        public string Community { get; set; } = string.Empty;
        public string StudType { get; set; } = string.Empty; // Hosteler, Bus-User, Day Sch.
        public string FirstGrad { get; set; } = string.Empty;
        public int HostelerId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ShortGender { get; set; } = string.Empty;
        public byte StudStat { get; set; }
        public byte ReqHostel { get; set; }
        public byte ReqTransport { get; set; }
        public int InstId { get; set; }
        public string SType { get; set; } = string.Empty;
        public string AdmnTypeShort { get; set; } = string.Empty;
        public string AdmnType { get; set; } = string.Empty;
        public byte IsClassRep { get; set; }
        public int RepSl { get; set; }
        public byte LoginAs { get; set; }
        public string ProfilePic { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for Student Login Response Data
    /// </summary>
    public class StudentLoginDataDto
    {
        public long LoginId { get; set; }
        public string LastLoginTime { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string RegisterNumber { get; set; } = string.Empty;
        public string ImpresCode { get; set; } = string.Empty;
        public string Quota { get; set; } = string.Empty;
        public string Community { get; set; } = string.Empty;
        public string StudType { get; set; } = string.Empty;
        public string FirstGrad { get; set; } = string.Empty;
        public int HostelerId { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ShortGender { get; set; } = string.Empty;
        public byte StudStat { get; set; }
        public byte ReqHostel { get; set; }
        public byte ReqTransport { get; set; }
        public int InstId { get; set; }
        public string SType { get; set; } = string.Empty;
        public string AdmnTypeShort { get; set; } = string.Empty;
        public string AdmnType { get; set; } = string.Empty;
        public byte IsClassRep { get; set; }
        public int RepSl { get; set; }
        public byte LoginAs { get; set; }
        public string ProfilePic { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
    }

    public class StudentLoginResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public StudentLoginDataDto? Data { get; set; }
        public string? ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
