using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface ISectionMasterRepository
    {
        Task<string> MergeSectionMasterAsync(SectionMaster sectionMaster);
        Task<(IEnumerable<SectionMaster> sections, IEnumerable<SchoolInfo> SchoolDetails)> FetchSectionMasterAsync();
    }
}
