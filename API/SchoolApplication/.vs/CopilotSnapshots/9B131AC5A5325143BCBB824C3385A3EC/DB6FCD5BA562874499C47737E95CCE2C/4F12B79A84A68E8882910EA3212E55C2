using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface ISubjectMasterRepository
    {
        Task<string> MergeSubjectMasterAsync(SubjectMaster subjectMaster);
        Task<(IEnumerable<SubjectMaster> subjects, IEnumerable<SchoolInfo> SchoolDetails)> FetchSubjectMasterAsync();
    }
}
