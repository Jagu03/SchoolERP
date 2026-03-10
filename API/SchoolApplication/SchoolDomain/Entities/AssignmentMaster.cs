namespace SchoolDomain.Entities
{
    public class AssignmentMaster
    {
        public int EditId { get; set; }
        public short AcadYearId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string AssignmentType { get; set; } = string.Empty; // Assignment / Homework
        public string Title { get; set; } = string.Empty;
        public string TitleDescription { get; set; } = string.Empty;
        public DateTime GivenDate { get; set; }
        public DateTime DueDate { get; set; }
        public int MaxMarks { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }
    }
}
