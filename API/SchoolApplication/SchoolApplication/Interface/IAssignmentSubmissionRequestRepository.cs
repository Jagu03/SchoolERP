using SchoolDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApplication.Interface
{
    public interface IAssignmentSubmissionRequestRepository
    {
        Task<string> MergeAssignmentSubmissionRequestAsync(AssignmentSubmissionRequest assignmentSubmissionRequest);
    }
}
