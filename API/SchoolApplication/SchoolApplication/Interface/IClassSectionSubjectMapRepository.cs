using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface IClassSectionSubjectMapRepository
    {
        Task<string> MergeClassSectionSubjectMapAsync(ClassSectionSubjectMap classSectionSubjectMap);
        Task<(IEnumerable<ClassSectionSubjectMap> classSectionSubjectMaps, IEnumerable<SchoolInfo> SchoolDetails)> FetchClassSectionSubjectMapAsync(int classId, byte acadYearId, int? sectionId);        
    }
}
