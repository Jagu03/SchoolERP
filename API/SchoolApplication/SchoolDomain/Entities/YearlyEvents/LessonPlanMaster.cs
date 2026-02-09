namespace SchoolDomain.Entities.YearlyEvents
{
    public class LessonPlanMaster
    {
        public int EditId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        public int StaffId { get; set; }
        public byte AcadYearId { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public string TopicDescription { get; set; } = string.Empty;
        public DateTime PlannedFromDate { get; set; }
        public DateTime PlannedToDate { get; set; }
        public byte Unit { get; set; }
        public string TeachingMethod { get; set; } = string.Empty;  
        public string StudentLearningMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public byte IsActive { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public short CreatedUserId { get; set; }
        public long LoginId { get; set; }

    }

    public class LessonPlanMasterView
    {
        public string Clsn { get; set; } = string.Empty;
        public string secfn { get; set; } = string.Empty;
        public string Subfn { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string Toptil { get; set; } = string.Empty;
        public DateTime PlanFd { get; set; }    
        public DateTime PlanTd { get; set; }
        public string Stustxt { get; set; } = string.Empty;
        public string rmk { get; set; } = string.Empty;
        public string yn { get; set; } = string.Empty;
        public int unit { get; set; }
        public string TechMtd { get; set; } = string.Empty;
        public string StudLrnMtd { get; set; } = string.Empty;
        public string TopDesc { get;set; } = string.Empty;
    }
}
