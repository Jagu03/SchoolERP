using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDomain.Entities
{
    public class AssignmentSubmissionRequest
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;



        public string FileType { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Remarks cannot exceed 100 characters.")]
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }
    }
}
