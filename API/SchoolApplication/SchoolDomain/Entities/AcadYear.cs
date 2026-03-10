using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SchoolDomain.Entities
{
    /// <summary>
    /// Academic Year entity representing a school academic year.
    /// </summary>
    public class AcadYear
    {        
        public byte AcadYearId { get; set; }

        [Required(ErrorMessage = "Year name is required")]
        [StringLength(100, ErrorMessage = "Year name cannot exceed 100 characters")]
        public string YearName { get; set; } = string.Empty;

        [Required(ErrorMessage = "From date is required")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To date is required")]
        public DateTime ToDate { get; set; }

        [Range(0, 1, ErrorMessage = "IsActive must be 0 or 1")]
        public byte IsActive { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Created User ID must be greater than 0")]
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }
    }
}
