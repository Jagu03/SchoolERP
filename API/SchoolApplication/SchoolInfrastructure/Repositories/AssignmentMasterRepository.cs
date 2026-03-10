using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace SchoolInfrastructure.Repositories
{
    public class AssignmentMasterRepository : BaseRepository, IAssignmentMasterRepository
    {  
        public AssignmentMasterRepository(IConfiguration configuration, ILogger<AssignmentMasterRepository> logger)
            : base(configuration, logger)
        {
            
        }

        public async Task<string> MergeAssignmentMasterAsync(AssignmentMaster assignmentMaster)
        {
            ArgumentNullException.ThrowIfNull(assignmentMaster);
            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@EditId", assignmentMaster.EditId);
                parameters.Add("@AcadYearId", assignmentMaster.AcadYearId);
                parameters.Add("@ClassId", assignmentMaster.ClassId);
                parameters.Add("@SectionId", assignmentMaster.SectionId);
                parameters.Add("@SubjectId", assignmentMaster.SubjectId);
                parameters.Add("@StaffId", assignmentMaster.StaffId);
                parameters.Add("@AssignmentType", assignmentMaster.AssignmentType);
                parameters.Add("@Title", assignmentMaster.Title);
                parameters.Add("@TitleDescription", assignmentMaster.TitleDescription);
                parameters.Add("@GivenDate", assignmentMaster.GivenDate);
                parameters.Add("@DueDate", assignmentMaster.DueDate);
                parameters.Add("@MaxMarks", assignmentMaster.MaxMarks);
                parameters.Add("@Remarks", assignmentMaster.Remarks);
                parameters.Add("@CreatedUserId", assignmentMaster.CreatedUserId);
                parameters.Add("@LoginId", assignmentMaster.LoginId);
                parameters.Add("@Result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeAssignment"
                    , parameters, commandType: System.Data.CommandType.StoredProcedure);

                return parameters.Get<string>("@Result") ?? string.Empty;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "MergeAssignmentMaster", $"EditId={assignmentMaster.EditId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "MergeAssignmentMaster", $"EditId={assignmentMaster.EditId}");
                throw;
            }
        }

        public async Task<IEnumerable<AssignmentMaster>> FetchAssignmentMasterAsync(int classId,int subjectId,short acadYearId)
        {
            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ClassId", classId, DbType.Int16);
                parameters.Add("@SubjectId", subjectId, DbType.Int16);
                parameters.Add("@AcadYearId", acadYearId, DbType.Int16);
                await using var multi = await connection.QueryMultipleAsync(
                    "[SchoolAcad].[FetchAssignments]", parameters,commandType: CommandType.StoredProcedure);

                var assignmentMasters = (await multi.ReadAsync<AssignmentMaster>()).ToList();

                Logger.LogInformation("Fetched {assignmentMasters} assignmentMasters successfully");
                return assignmentMasters;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "FetchAssignmentMaster");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "FetchAssignmentMaster");
                throw;
            }
        }
    }
}