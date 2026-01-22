using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDomain.Entities
{
    public class SubjectMaster
    {
        public int subid { get; set; }

        [Required(ErrorMessage = "Subject name is required.")]
        public string txt { get; set; } = string.Empty;
        public string subcode { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Short name cannot exceed 20 characters.")]
        public string shorttxt { get; set; } = string.Empty;
        public byte isc { get; set; }
        public short stypid { get; set; }
        public string ftxt { get; set; } = string.Empty;
        public string rmk { get; set; } = string.Empty;
        public short cuid { get; set; }
        public long Logid { get; set; }
    }
}
