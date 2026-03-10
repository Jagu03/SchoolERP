using SchoolDomain.Entities;

namespace SchoolApplication.Interface
{
    public interface IClassSectionAllocationRepository
    {
        Task<string> MergeAllocateClassSectionAsync(ClassSectionAllocation classSectionAllocation);
        Task<(IEnumerable<ClassSectionAllocation> allocations, IEnumerable<SchoolInfo> SchoolDetails)> FetchClassSectionAllocationsAsync();
    }
}
