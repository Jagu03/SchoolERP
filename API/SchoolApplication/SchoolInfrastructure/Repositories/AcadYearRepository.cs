using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class AcadYearRepository : IAcadYearRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<AcadYearRepository> _logger;
        public AcadYearRepository(IConfiguration configuration, ILogger<AcadYearRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeAcadYearAsync(AcadYear acadYear)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var parameters = new DynamicParameters();
            parameters.Add("@EditId", acadYear.AcadYearId);
            parameters.Add("@YearName", acadYear.YearName);
            parameters.Add("@FromDate", acadYear.FromDate);
            parameters.Add("@ToDate", acadYear.ToDate);
            parameters.Add("@IsActive", acadYear.IsActive);
            parameters.Add("@CreatedUserId", acadYear.CreatedUserId);
            parameters.Add("@LoginId", acadYear.LoginId);
            parameters.Add("@result", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("SchoolAcad.MergeAcademicYears",
                parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<string>("@result") ?? string.Empty;
        }

        public async Task<(IEnumerable<AcadYear> acadYears, IEnumerable<SchoolInfo> SchoolDetails)> FetchAllAcadYearAsync()
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var multi = await connection.QueryMultipleAsync(
                "[SchoolAcad].[FetchAllAcadYear]", commandType: CommandType.StoredProcedure);

            var acadYears = (await multi.ReadAsync<AcadYear>()).ToList();

            IEnumerable<SchoolInfo> schoolDetails = Enumerable.Empty<SchoolInfo>();
            if (!multi.IsConsumed)
            {
                schoolDetails = (await multi.ReadAsync<SchoolInfo>()).ToList();
            }
            return (acadYears, schoolDetails);
        }

    }
}

