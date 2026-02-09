using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApplication.Interface
{
    public interface IAssignmentMasterRepository 
    {
        Task<string> MergeAssignmentMasterAsync(SchoolDomain.Entities.AssignmentMaster assignmentMaster);
    }
}
