using System.ComponentModel.DataAnnotations;

namespace SchoolDomain.Entities
{
    public class ClassSectionAllocation
    {
        public int AllocationId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public byte AcadYearId { get; set; }
        public string YearName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }
    }
}
