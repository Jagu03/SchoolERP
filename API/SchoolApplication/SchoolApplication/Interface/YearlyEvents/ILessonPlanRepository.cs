namespace SchoolApplication.Interface.YearlyEvents
{
    public interface ILessonPlanRepository
    {
        Task<string> MergeLessonPlanAsync(SchoolDomain.Entities.YearlyEvents.LessonPlanMaster lessonPlanMaster);
        Task<IEnumerable<SchoolDomain.Entities.YearlyEvents.LessonPlanMasterView>> FetchLessonPlanAsync(int ClassId, int subjectid, short acadYearId);  
    }
}
