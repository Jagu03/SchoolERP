using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class SyllabusFinalizationRepository : ISyllabusFinalizationRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SyllabusFinalizationRepository> _logger;

        public SyllabusFinalizationRepository(IConfiguration configuration, ILogger<SyllabusFinalizationRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeSyllabusFinalizationAsync(SyllabusFinalization syllabusFinalization)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            {
                parameters.Add("@EditId", syllabusFinalization.SyllabusFinalId);
                parameters.Add("@ClassId", syllabusFinalization.ClassId);
                parameters.Add("@SectionId", syllabusFinalization.SectionId);
                parameters.Add("@SubjectId", syllabusFinalization.SubjectId);
                parameters.Add("@AcadYearId", syllabusFinalization.AcadYearId);
                parameters.Add("@IsFinalized", syllabusFinalization.IsFinalized);
                parameters.Add("@Remarks", syllabusFinalization.Remarks);
                parameters.Add("@CreatedUserId", syllabusFinalization.CreatedUserId);
                parameters.Add("@LoginId", syllabusFinalization.LoginId);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeSyllabusFinalization",
                parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@result") ?? string.Empty;
            }
        }

        //public async Task<(IEnumerable<SyllabusFinalization> syllabusFinalization,IEnumerable<SchoolInfo> SchoolDetails)> FetchSyllabusFinalizationAsync(int classId, short acadYearId, int? sectionId)
        //{
        //    await using var connection = new SqlConnection(_connectionString);
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@ClassId", classId, DbType.Int32);
        //    parameters.Add("@AcadYearId", acadYearId, DbType.Int16);
        //    parameters.Add("@SectionId", sectionId, DbType.Int16);

        //    await using var multi = await connection.QueryMultipleAsync(
        //        "[SchoolAcad].[FetchSyllabusFinalization]",
        //        parameters,
        //        commandType: CommandType.StoredProcedure);

        //    // Materialize the first result set immediately
        //    var syllabusFinalization = (await multi.ReadAsync<SyllabusFinalization>()).ToList();

        //    // Try to read a second result set only if available
        //    IEnumerable<SchoolInfo> schoolDetails = Enumerable.Empty<SchoolInfo>();
        //    if (!multi.IsConsumed)
        //    {
        //        schoolDetails = (await multi.ReadAsync<SchoolInfo>()).ToList();
        //    }

        //    return (syllabusFinalization, schoolDetails);
        //}
        public async Task<IEnumerable<SyllabusFinalizationView>> FetchSyllabusFinalizationAsync(int classId, short acadYearId, int? sectionId)
        {
            await using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@ClassId", classId, DbType.Int32);
            parameters.Add("@AcadYearId", acadYearId, DbType.Int16);
            parameters.Add("@SectionId", sectionId, DbType.Int32);

            return await connection.QueryAsync<SyllabusFinalizationView>(
                "[SchoolAcad].[FetchSyllabusFinalization]",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

    }
}
