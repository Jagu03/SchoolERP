using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class ClassMasterRepository : BaseRepository, IClassMasterRepository
    {
        public ClassMasterRepository(IConfiguration configuration, ILogger<ClassMasterRepository> logger)
            : base(configuration, logger)
        {
        }

        public async Task<string> MergeClassMasterAsync(ClassMaster classMaster)
        {
            ArgumentNullException.ThrowIfNull(classMaster);

            try
            {
                await using var connection = CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@EditId", classMaster.ClassId);
                parameters.Add("@ClassName", classMaster.ClassName);
                parameters.Add("@DisplayOrder", classMaster.DisplayOrder);
                parameters.Add("@IsActive", classMaster.IsActive);
                parameters.Add("@Remarks", classMaster.Remarks);
                parameters.Add("@CreatedUserId", classMaster.CreatedUserId);
                parameters.Add("@LoginId", classMaster.LoginId);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeClassMaster",
                    parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@result") ?? string.Empty;
            }
            catch (SqlException ex)
            {
                throw HandleDatabaseError(ex, "MergeClassMaster", $"ClassId={classMaster.ClassId}");
            }
            catch (Exception ex)
            {
                LogUnexpectedError(ex, "MergeClassMaster", $"ClassId={classMaster.ClassId}");
                throw;
            }
        }

        public async Task<(IEnumerable<ClassMaster> classes, IEnumerable<SchoolInfo> School)> FetchClassMasterAsync()
        {
            try
            {
                await using var connection = CreateConnection();
                await using var multi = await connection.QueryMultipleAsync(
                    "[SchoolAcad].[FetchClassMaster]", commandType: CommandType.StoredProcedure);

                var classes = (await multi.ReadAsync<ClassMaster>()).ToList();

                IEnumerable<SchoolInfo> school = Enumerable.Empty<SchoolInfo>();
                if (!multi.IsConsumed)
                {
                    school = (await multi.ReadAsync<SchoolInfo>()).ToList();
                }

                Logger.LogInformation("Fetched {ClassCount} classes successfully", classes.Count);
                return (classes, school);
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


