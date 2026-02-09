namespace SchoolDomain.Entities
{
    public class ClassroomTeaching
    {
        public int EditId { get; set; }
        public short AcadYearId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        public int StaffId { get; set; }
        public DateTime TeachingDate { get; set; }
        public byte PeriodNo { get; set; }
        public byte UnitNo { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public string TopicDescription { get; set; } = string.Empty;
        public string TeachingMethod { get; set; } = string.Empty;
        public string StudentLearningMethod { get; set; } = string.Empty;
        public byte IsCompleted { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public short UserId { get; set; }
        public long LoginId { get; set; }

    }
    public class ClassroomTeachingview
    {
        public int ClassroomTeachingId { get; set; }
        public DateTime TeachingDate { get; set; }
        public int PeriodNo { get; set; }
        public byte UnitNo { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public string TopicDescription { get; set; } = string.Empty;
        public string TeachingMethod { get; set; } = string.Empty;
        public string StudentLearningMethod { get; set; } = string.Empty;
        public byte IsCompleted { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty; 
    }
}
