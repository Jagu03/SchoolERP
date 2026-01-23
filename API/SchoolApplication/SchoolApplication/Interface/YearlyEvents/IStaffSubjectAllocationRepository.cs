using SchoolDomain.Entities;
using SchoolDomain.Entities.YearlyEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApplication.Interface.YearlyEvents
{
    public interface IStaffSubjectAllocationRepository 
    {
        Task<string> MergeStaffSubjectAllocationAsync(StaffSubjectAllocation staffSubjectAllocation);
        Task<IEnumerable<StaffSubjectAllocationview>> FetchStaffSubjectAllocationAsync(byte acadYearId);
    }
}
