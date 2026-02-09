using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApplication.Interface
{
    public interface IClassroomTeachingRepository
    {
        Task<string> MergeClassroomTeachingAsync(SchoolDomain.Entities.ClassroomTeaching classroomTeaching);
        Task<IEnumerable<SchoolDomain.Entities.ClassroomTeachingview>> GetClassroomTeachingByDateAsync(short acadYearId, int classId, int subjectId, DateTime fromdate, DateTime todate);
    }
}
