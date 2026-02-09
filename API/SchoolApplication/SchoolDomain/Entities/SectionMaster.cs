using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDomain.Entities
{
    public class SectionMaster
    {
        public int SectionId { get; set; }

        [Required(ErrorMessage = "Section name is required.")]
        public string SectionName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }

    }
}
