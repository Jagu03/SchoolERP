using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface IClassMasterRepository
    {
        Task<string> MergeClassMasterAsync(ClassMaster classMaster);
        Task<(IEnumerable<ClassMaster> classes, IEnumerable<SchoolInfo> School)> FetchClassMasterAsync();
    }
}
