namespace SchoolDomain.Entities
{
    public class SyllabusFinalization
    {
        public int SyllabusFinalId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        public short AcadYearId { get; set; }
        public byte IsFinalized { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }

    }
    public class SyllabusFinalizationView
    {
        public int SyllabusFinalId { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public byte IsFinalized { get; set; }
        public DateTime? FinalizedDate { get; set; }
        public string Remarks { get; set; }
    }
}
