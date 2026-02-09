namespace SchoolDomain.Entities
{
    public class ClassSectionSubjectMap
    {
        public int Mapid { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int SectionId { get; set; }
        public byte AcadYearId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public byte IsActive { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId  { get; set; }
    }
}


