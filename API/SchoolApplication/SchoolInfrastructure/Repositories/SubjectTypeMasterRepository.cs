using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using SchoolInfrastructure.Repositories;
using System.Data;

namespace SchoolInfrastructure
{
    public class SubjectTypeMasterRepository : BaseRepository, ISubjectTypeMasterRepository
    {
        public SubjectTypeMasterRepository(IConfiguration configuration, ILogger<SubjectTypeMasterRepository> logger)
            : base(configuration, logger)
        {
        }

        public async Task<string> MergeSubjectTypeMasterAsync(SubjectTypeMaster subjectTypeMaster)
        {
            ArgumentNullException.ThrowIfNull(subjectTypeMaster);

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@EditId", subjectTypeMaster.SubjTypeId);
                parameters.Add("@FullName", subjectTypeMaster.FullName);
                parameters.Add("@ShortName", subjectTypeMaster.ShortName);
                parameters.Add("@ShowAs", subjectTypeMaster.ShowAs);
                parameters.Add("@IsActive", subjectTypeMaster.IsActive);
                parameters.Add("@CreatedUserId", subjectTypeMaster.CreatedUserId);
                parameters.Add("@LoginId", subjectTypeMaster.LoginId);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeSubjectTypeMaster",
                    parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@result") ?? string.Empty;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "MergeSubjectTypeMaster", $"SubjTypeId={subjectTypeMaster.SubjTypeId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "MergeSubjectTypeMaster", $"SubjTypeId={subjectTypeMaster.SubjTypeId}");
                throw;
            }
        }

        public async Task<IEnumerable<SubjectTypeMaster>> FetchSubjectTypeMasterAsync()
        {
            try
            {
                await using var connection = CreateConnection();
                await using var multi = await connection.QueryMultipleAsync(
                    "[SchoolAcad].[FetchTypeSubjectMaster]", commandType: CommandType.StoredProcedure);

                var subMasterTypes = (await multi.ReadAsync<SubjectTypeMaster>()).ToList();

                Logger.LogInformation("Fetched {subjectTypeMaster} subjectTypeMaster successfully");
                return subMasterTypes;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "FetchClassMaster");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "FetchClassMaster");
                throw;
            }
        }
    }
}
