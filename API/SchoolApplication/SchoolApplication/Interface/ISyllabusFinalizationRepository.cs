using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface ISyllabusFinalizationRepository
    {
        Task<string> MergeSyllabusFinalizationAsync(SyllabusFinalization syllabusFinalization);
        Task<IEnumerable<SyllabusFinalizationView>> FetchSyllabusFinalizationAsync(int classId, short acadYearId, int? sectionId);
    }
}
