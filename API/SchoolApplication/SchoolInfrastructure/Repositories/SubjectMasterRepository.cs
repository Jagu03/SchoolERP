using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class SubjectMasterRepository : ISubjectMasterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SubjectMasterRepository> _logger;

        public SubjectMasterRepository(IConfiguration configuration, ILogger<SubjectMasterRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeSubjectMasterAsync(SubjectMaster subjectMaster)
        {
            ArgumentNullException.ThrowIfNull(subjectMaster);

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@EditId", subjectMaster.SubjectId);
                parameters.Add("@SubjectName", subjectMaster.SubjectName);
                parameters.Add("@SubjectCode", subjectMaster.SubjectCode);
                parameters.Add("@ShortName", subjectMaster.ShortName);
                parameters.Add("@IsChoice", subjectMaster.IsChoice);
                parameters.Add("@SubjTypeId", subjectMaster.SubjTypeId);
                parameters.Add("@Remarks", subjectMaster.Remarks);
                parameters.Add("@CreatedUserId", subjectMaster.CreatedUserId);
                parameters.Add("@LoginId", subjectMaster.LoginId);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeSubjectMaster",
                    parameters, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Subject merged successfully. SubjectID: {SubjectId}", subjectMaster.SubjectId);
                return parameters.Get<string>("@result") ?? string.Empty;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error merging SubjectMaster (SubjectID={SubjectId})", subjectMaster.SubjectId);
                throw new InvalidOperationException($"An error occurred while processing subject. Details: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging SubjectMaster (SubjectID={SubjectId})", subjectMaster.SubjectId);
                throw;
            }
        }
        public async Task<(IEnumerable<SubjectMaster> subjects, IEnumerable<SchoolInfo> SchoolDetails)> FetchSubjectMasterAsync()
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                await using var multi = await connection.QueryMultipleAsync(
                    "[SchoolAcad].[FetchSubjectMaster]", commandType: CommandType.StoredProcedure);

                var subjects = (await multi.ReadAsync<SubjectMaster>()).ToList();

                IEnumerable<SchoolInfo> schoolDetails = Enumerable.Empty<SchoolInfo>();
                if (!multi.IsConsumed)
                {
                    schoolDetails = (await multi.ReadAsync<SchoolInfo>()).ToList();
                }

                _logger.LogInformation("Fetched {SubjectCount} subjects successfully", subjects.Count);
                return (subjects, schoolDetails);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error fetching SubjectMaster");
                throw new InvalidOperationException("An error occurred while fetching subjects.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching SubjectMaster");
                throw;
            }
        }

    }
}
