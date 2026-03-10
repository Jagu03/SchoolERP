using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class AssignmentSubmissionRequestRepository : BaseRepository , IAssignmentSubmissionRequestRepository
    {
        public AssignmentSubmissionRequestRepository(IConfiguration configuration, ILogger<AssignmentSubmissionRequestRepository> logger)
            : base(configuration, logger)
        {
        }

        public async Task<string> MergeAssignmentSubmissionRequestAsync(AssignmentSubmissionRequest assignmentSubmissionRequest)
        {
            ArgumentNullException.ThrowIfNull(assignmentSubmissionRequest);

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AssignmentId", assignmentSubmissionRequest.AssignmentId);
                parameters.Add("@StudentId", assignmentSubmissionRequest.StudentId);
                parameters.Add("@FileName", assignmentSubmissionRequest.FileName);
                parameters.Add("@FilePath", assignmentSubmissionRequest.FilePath);
                parameters.Add("@FileType", assignmentSubmissionRequest.FileType);
                parameters.Add("@Remarks", assignmentSubmissionRequest.Remarks);
                parameters.Add("@CreatedUserId", assignmentSubmissionRequest.CreatedUserId);
                parameters.Add("@LoginId", assignmentSubmissionRequest.LoginId);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeAssignmentSubmission",
                    parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@result") ?? string.Empty;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "MergeAssignmentSubmission", $"AssignmentId={assignmentSubmissionRequest.AssignmentId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "MergeAssignmentSubmission", $"AssignmentId={assignmentSubmissionRequest.AssignmentId}");
                throw;
            }
        }
    }
}
