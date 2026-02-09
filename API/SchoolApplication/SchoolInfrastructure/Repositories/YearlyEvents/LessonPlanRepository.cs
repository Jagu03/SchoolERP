using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface.YearlyEvents;
using SchoolDomain.Entities.YearlyEvents;

namespace SchoolInfrastructure.Repositories.YearlyEvents
{
    public class LessonPlanRepository : ILessonPlanRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<LessonPlanRepository> _logger;

        public LessonPlanRepository(IConfiguration configuration, ILogger<LessonPlanRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeLessonPlanAsync(LessonPlanMaster lessonPlanMaster)
        {
            await using var connection = new SqlConnection(_connectionString);
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EditId", lessonPlanMaster.EditId);
                parameters.Add("@ClassId", lessonPlanMaster.ClassId);
                parameters.Add("@SectionId", lessonPlanMaster.SectionId);
                parameters.Add("@SubjectId", lessonPlanMaster.SubjectId);
                parameters.Add("@StaffId", lessonPlanMaster.StaffId);
                parameters.Add("@AcadYearId", lessonPlanMaster.AcadYearId);
                parameters.Add("@TopicTitle", lessonPlanMaster.TopicTitle);
                parameters.Add("@TopicDescription", lessonPlanMaster.TopicDescription);
                parameters.Add("@PlannedFromDate", lessonPlanMaster.PlannedFromDate);
                parameters.Add("@PlannedToDate", lessonPlanMaster.PlannedToDate);
                parameters.Add("@Unit", lessonPlanMaster.Unit);
                parameters.Add("@TeachingMethod", lessonPlanMaster.TeachingMethod);
                parameters.Add("@StudentLearningMethod", lessonPlanMaster.StudentLearningMethod);
                parameters.Add("@Status", lessonPlanMaster.Status);
                parameters.Add("@IsActive", lessonPlanMaster.IsActive);
                parameters.Add("@Remarks", lessonPlanMaster.Remarks);
                parameters.Add("@CreatedUserId", lessonPlanMaster.CreatedUserId);
                parameters.Add("@LoginId", lessonPlanMaster.LoginId);
                parameters.Add("@Result", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);

                await connection.ExecuteAsync("SchoolAcad.MergeLessonPlan"
                    , parameters, commandType: System.Data.CommandType.StoredProcedure);

                return parameters.Get<string>("@Result") ?? string.Empty;
            }
              
        }

        public async Task<IEnumerable<LessonPlanMasterView>> FetchLessonPlanAsync(int ClassId,int subjectid,short acadYearId)
        {
            await using var connection = new SqlConnection(_connectionString);
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ClassId", ClassId);
                parameters.Add("@SubjectId", subjectid);
                parameters.Add("@AcadYearId", acadYearId);
                var result = await connection.QueryAsync<LessonPlanMasterView>("SchoolAcad.FetchLessonPlan",
                    parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
