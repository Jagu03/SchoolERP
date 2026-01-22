using System.ComponentModel.DataAnnotations;

namespace SchoolDomain.Entities
{
    public class ClassSectionAllocation
    {
        public int alid { get; set; }
        public int clsid { get; set; }
        public int secid { get; set; }
        public byte ayid { get; set; }
        public byte isc { get; set; }

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string rmk { get; set; } = string.Empty;
        public short cuid { get; set; }
        public long Logid { get; set; }
    }
}
