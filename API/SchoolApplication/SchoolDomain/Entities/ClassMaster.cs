using System.ComponentModel.DataAnnotations;

namespace SchoolDomain.Entities
{
    public class ClassMaster
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Class name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Class name must be between 1 and 100 characters.")]
        public string ClassName { get; set; } = string.Empty;

        [Range(0, 99, ErrorMessage = "Display order must be between 0 and 99.")]
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        [StringLength(1000, ErrorMessage = "Remarks cannot exceed 1000 characters.")]
        public string? Remarks { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Created User ID must be greater than 0.")]
        public short CreatedUserId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Login ID must be greater than 0.")]
        public long LoginId { get; set; }
    }

    public class SchoolInfo
    {
        public int semPeriodid { get; set; }
        public string semPeriodName { get; set; } = string.Empty;
        public string AcadYearName { get; set; } = string.Empty;
        public string insFullname { get; set; } = string.Empty;
        public string insShortName { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
    }
}
