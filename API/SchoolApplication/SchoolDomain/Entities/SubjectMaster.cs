using System.ComponentModel.DataAnnotations;

namespace SchoolDomain.Entities
{
    public class SubjectMaster
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject name is required.")]
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Short name cannot exceed 50 characters.")]
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public bool IsChoice { get; set; }
        public short SubjTypeId { get; set; }

        [StringLength(100, ErrorMessage = "Remarks cannot exceed 100 characters.")]
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long LoginId { get; set; }
    }
}
