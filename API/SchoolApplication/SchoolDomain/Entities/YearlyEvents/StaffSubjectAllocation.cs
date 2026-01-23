namespace SchoolDomain.Entities.YearlyEvents
{
    public class StaffSubjectAllocation
    {
        public int AllocationId { get; set; }
        public int StaffId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        public short AcadYearId { get; set; }
        public byte IsActive { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }
    }

    public class StaffSubjectAllocationview
    {
        public int AllocationId { get; set; }
        public string txt { get; set; } = string.Empty;
        public string clsfn { get; set; } = string.Empty;
        public string secname { get; set; } = string.Empty;
        public string subfn { get; set; } = string.Empty;
        public byte st { get; set; }
        public string rmk { get; set; } = string.Empty;
        public DateTime? cdt { get; set; }
        public string yn { get; set; } = string.Empty;

    }
}
