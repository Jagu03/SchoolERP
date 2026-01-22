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
    public class ClassSectionSubjectMapRepository : IClassSectionSubjectMapRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ClassSectionSubjectMapRepository> _logger;

        public ClassSectionSubjectMapRepository (IConfiguration configuration, ILogger<ClassSectionSubjectMapRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeClassSectionSubjectMapAsync(ClassSectionSubjectMap classSectionSubjectMap)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            {
                parameters.Add("@EditId", classSectionSubjectMap.Mapid);
                parameters.Add("@ClassId", classSectionSubjectMap.clsid);
                parameters.Add("@SectionId", classSectionSubjectMap.secid);
                parameters.Add("@SubjectId", classSectionSubjectMap.subid);
                parameters.Add("@AcadYearId", classSectionSubjectMap.ayid);
                parameters.Add("@IsActive", classSectionSubjectMap.isn);
                parameters.Add("@Remarks", classSectionSubjectMap.rmk);
                parameters.Add("@CreatedUserId", classSectionSubjectMap.cuid);
                parameters.Add("@LoginId", classSectionSubjectMap.Logid);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeClassSectionSubjectMap",
                parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@result") ?? string.Empty;
            }
        }

        public async Task<(IEnumerable<ClassSectionSubjectMap> classSectionSubjectMaps, IEnumerable<SchoolInfo> SchoolDetails)> FetchClassSectionSubjectMapAsync(int classId, short acadYearId, int? sectionId)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@ClassId", classId, DbType.Int16);
            parameters.Add("@SectionId", sectionId, DbType.Int16);
            parameters.Add("@AcadYearId", acadYearId, DbType.Int16);

            await using var multi = await connection.QueryMultipleAsync(
                "[SchoolAcad].[FetchClassSectionSubjectMap]",
                parameters,
                commandType: CommandType.StoredProcedure);

            // Materialize the first result set immediately
            var classSectionSubjectMaps = (await multi.ReadAsync<ClassSectionSubjectMap>()).ToList();

            // Try to read a second result set only if available
            IEnumerable<SchoolInfo> schoolDetails = Enumerable.Empty<SchoolInfo>();
            if (!multi.IsConsumed)
            {
                schoolDetails = (await multi.ReadAsync<SchoolInfo>()).ToList();
            }

            return (classSectionSubjectMaps, schoolDetails);

        }
    }
}


