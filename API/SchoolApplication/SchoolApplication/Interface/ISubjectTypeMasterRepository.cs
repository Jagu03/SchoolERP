using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface ISubjectTypeMasterRepository
    {
        Task<string> MergeSubjectTypeMasterAsync(SubjectTypeMaster subjectTypeMaster);
        Task<IEnumerable<SubjectTypeMaster>> FetchSubjectTypeMasterAsync();
    }
}
