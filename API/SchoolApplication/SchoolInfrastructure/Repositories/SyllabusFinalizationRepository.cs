using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class SyllabusFinalizationRepository : BaseRepository, ISyllabusFinalizationRepository
    {
        public SyllabusFinalizationRepository(IConfiguration configuration, ILogger<SyllabusFinalizationRepository> logger)
            : base(configuration, logger)
        {
        }

        public async Task<string> MergeSyllabusFinalizationAsync(SyllabusFinalization syllabusFinalization)
        {
            ArgumentNullException.ThrowIfNull(syllabusFinalization);

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
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
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "MergeSyllabusFinalization", $"SyllabusFinalId={syllabusFinalization.SyllabusFinalId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "MergeSyllabusFinalization", $"SyllabusFinalId={syllabusFinalization.SyllabusFinalId}");
                throw;
            }
        }

        public async Task<IEnumerable<SyllabusFinalizationView>> FetchSyllabusFinalizationAsync(int classId, short acadYearId, int? sectionId)
        {
            try
            {
                await using var connection = CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@ClassId", classId, DbType.Int32);
                parameters.Add("@AcadYearId", acadYearId, DbType.Int16);
                parameters.Add("@SectionId", sectionId, DbType.Int32);

                var result = await connection.QueryAsync<SyllabusFinalizationView>(
                    "[SchoolAcad].[FetchSyllabusFinalization]",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                Logger.LogInformation("Fetched {Count} syllabus finalization records", result.Count());
                return result;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "FetchSyllabusFinalization", $"ClassId={classId}, AcadYearId={acadYearId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "FetchSyllabusFinalization", $"ClassId={classId}, AcadYearId={acadYearId}");
                throw;
            }
        }
    }
}


