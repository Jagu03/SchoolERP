using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System.Data;

namespace SchoolInfrastructure.Repositories
{
    public class ClassSectionAllocationRepository : IClassSectionAllocationRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ClassSectionAllocationRepository> _logger; 

        public ClassSectionAllocationRepository(IConfiguration configuration, ILogger<ClassSectionAllocationRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeAllocateClassSectionAsync(ClassSectionAllocation classSectionAllocation)
        {
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@EditId", classSectionAllocation.AllocationId);
            parameters.Add("@ClassId", classSectionAllocation.ClassId);
            parameters.Add("@SectionId", classSectionAllocation.SectionId);
            parameters.Add("@AcadYearId", classSectionAllocation.AcadYearId);
            parameters.Add("@IsActive", classSectionAllocation.IsActive);
            parameters.Add("@Remarks", classSectionAllocation.Remarks);
            parameters.Add("@CreatedUserId", classSectionAllocation.CreatedUserId);
            parameters.Add("@LoginId", classSectionAllocation.LoginId);
            parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("SchoolAcad.MergeClassSectionAllocation",
                parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<string>("@result") ?? string.Empty;
        }

        public async Task<(IEnumerable<ClassSectionAllocation> allocations, IEnumerable<SchoolInfo> SchoolDetails)> FetchClassSectionAllocationsAsync()
        {
            await using var connection = new SqlConnection(_connectionString); 
            await using var multi = await connection.QueryMultipleAsync(
                "[SchoolAcad].[FetchClassSectionAllocation]",
               
                commandType: CommandType.StoredProcedure);

            // Materialize the first result set immediately
            var allocations = (await multi.ReadAsync<ClassSectionAllocation>()).ToList();

            // Try to read a second result set only if available
            IEnumerable<SchoolInfo> schoolDetails = Enumerable.Empty<SchoolInfo>();
            if (!multi.IsConsumed)
            {
                schoolDetails = (await multi.ReadAsync<SchoolInfo>()).ToList();
            }

            return (allocations, schoolDetails);
        }
    }
}

