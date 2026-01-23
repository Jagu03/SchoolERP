using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface.YearlyEvents;
using SchoolDomain.Entities;
using SchoolDomain.Entities.YearlyEvents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SchoolInfrastructure.Repositories.YearlyEvents
{
    public class StaffSubjectAllocationRepository : IStaffSubjectAllocationRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<StaffSubjectAllocationRepository> _logger;

        public StaffSubjectAllocationRepository(IConfiguration configuration, ILogger<StaffSubjectAllocationRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeStaffSubjectAllocationAsync(StaffSubjectAllocation staffSubjectAllocation)
        {
            await using var connection = new SqlConnection(_connectionString);
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EditId", staffSubjectAllocation.AllocationId);
                parameters.Add("@StaffId", staffSubjectAllocation.StaffId);
                parameters.Add("@ClassId", staffSubjectAllocation.ClassId);
                parameters.Add("@SectionId", staffSubjectAllocation.SectionId);
                parameters.Add("@SubjectId", staffSubjectAllocation.SubjectId);
                parameters.Add("@AcadYearId", staffSubjectAllocation.AcadYearId);
                parameters.Add("@IsActive", staffSubjectAllocation.IsActive);
                parameters.Add("@Remarks", staffSubjectAllocation.Remarks);
                parameters.Add("@CreatedUserId", staffSubjectAllocation.CreatedUserId);
                parameters.Add("@LoginId", staffSubjectAllocation.LoginId);
                parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("SchoolAcad.MergeTeacherSubjectAllocation",
                parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@result") ?? string.Empty;
            }
        }

        public async Task<IEnumerable<StaffSubjectAllocationview>> FetchStaffSubjectAllocationAsync(byte acadYearId)
        {
           await using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@AcadYearId", acadYearId, DbType.Int16);

            return await connection.QueryAsync<StaffSubjectAllocationview>(
                "[SchoolAcad].[FetchStaffSubjectAllocation]",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
    

