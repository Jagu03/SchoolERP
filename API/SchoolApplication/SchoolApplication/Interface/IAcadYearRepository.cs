using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface IAcadYearRepository
    {
        Task<string> MergeAcadYearAsync (AcadYear acadYear);
        Task<(IEnumerable<AcadYear> acadYears, IEnumerable<SchoolInfo> SchoolDetails)> FetchAllAcadYearAsync();
    }
}
