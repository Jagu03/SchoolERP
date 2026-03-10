using System.ComponentModel.DataAnnotations;

namespace SchoolDomain.Entities
{
    public class SubjectTypeMaster
    {
        public byte SubjTypeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string ShowAs { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Created User ID must be greater than 0.")]
        public short CreatedUserId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Login ID must be greater than 0.")]
        public long LoginId { get; set; }
    }
}
