using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolInfrastructure.Repositories
{
    public class ClassroomTeachingRepository : IClassroomTeachingRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ClassroomTeachingRepository> _logger;

        public ClassroomTeachingRepository(IConfiguration configuration, ILogger<ClassroomTeachingRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeClassroomTeachingAsync(ClassroomTeaching classroomTeaching)
        {
            await using var connection = new SqlConnection(_connectionString);
            {
                var parameters = new DynamicParameters(classroomTeaching);
                {
                    new SqlParameter("@EditId", classroomTeaching.EditId);
                    new SqlParameter("@AcadYearId", classroomTeaching.AcadYearId);
                    new SqlParameter("@ClassId", classroomTeaching.ClassId);
                    new SqlParameter("@SectionId", classroomTeaching.SectionId);
                    new SqlParameter("@SubjectId", classroomTeaching.SubjectId);
                    new SqlParameter("@StaffId", classroomTeaching.StaffId);
                    new SqlParameter("@TeachingDate", classroomTeaching.TeachingDate);
                    new SqlParameter("@PeriodNo", classroomTeaching.PeriodNo);
                    new SqlParameter("@UnitNo", classroomTeaching.UnitNo);
                    new SqlParameter("@TopicTitle", classroomTeaching.TopicTitle ?? string.Empty);
                    new SqlParameter("@TopicDescription", classroomTeaching.TopicDescription ?? string.Empty);
                    new SqlParameter("@TeachingMethod", classroomTeaching.TeachingMethod ?? string.Empty);
                    new SqlParameter("@StudentLearningMethod", classroomTeaching.StudentLearningMethod ?? string.Empty);
                    new SqlParameter("@IsCompleted", classroomTeaching.IsCompleted);
                    new SqlParameter("@Remarks", classroomTeaching.Remarks ?? string.Empty);
                    new SqlParameter("@UserId", classroomTeaching.UserId);
                    new SqlParameter("@LoginId", classroomTeaching.LoginId);
                    parameters.Add("@Result", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);

                    await connection.ExecuteAsync("SchoolAcad.MergeClassroomTeaching"
                        , parameters, commandType: System.Data.CommandType.StoredProcedure);

                    return parameters.Get<string>("@Result") ?? string.Empty;

                }
            }
        }

        public async Task<IEnumerable<ClassroomTeachingview>> GetClassroomTeachingByDateAsync(short acadYearId, int classId, int subjectId, DateTime fromdate, DateTime todate)
        {
            await using var connection = new SqlConnection(_connectionString);
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AcadYearId", acadYearId);
                parameters.Add("@ClassId", classId);
                parameters.Add("@SubjectId", subjectId);
                parameters.Add("@FromDate", fromdate);
                parameters.Add("@ToDate", todate);
                var result = await connection.QueryAsync<ClassroomTeachingview>("SchoolAcad.FetchClassroomTeaching"
                    , parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
    }
}