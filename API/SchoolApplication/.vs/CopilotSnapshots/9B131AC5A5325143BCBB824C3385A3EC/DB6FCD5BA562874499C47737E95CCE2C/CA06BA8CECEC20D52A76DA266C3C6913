using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolInfrastructure.Repositories
{
    public class SubjectMasterRepository : ISubjectMasterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SubjectMasterRepository> _logger;  
        
        public SubjectMasterRepository(IConfiguration configuration,ILogger<SubjectMasterRepository> logger)
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
            await using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@EditId", subjectMaster.subid);
            parameters.Add("@SubjectName", subjectMaster.txt);
            parameters.Add("@SubjectCode", subjectMaster.subcode);
            parameters.Add("@ShortName", subjectMaster.shorttxt);
            parameters.Add("@IsChoice", subjectMaster.isc);
            parameters.Add("@SubjTypeId", subjectMaster.stypid);
            parameters.Add("@Remarks", subjectMaster.rmk);
            parameters.Add("@CreatedUserId", subjectMaster.cuid);
            parameters.Add("@LoginId", subjectMaster.Logid);
            parameters.Add("@result", dbType: DbType.String, size: 350, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("SchoolAcad.MergeSubjectMaster",
                parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<string>("@result") ?? string.Empty;

        }
        public async Task<(IEnumerable<SubjectMaster> subjects, IEnumerable<SchoolInfo> SchoolDetails)> FetchSubjectMasterAsync()
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var multi = await connection.QueryMultipleAsync(
                "[SchoolAcad].[FetchSubjectMaster]", commandType: CommandType.StoredProcedure);

            var subjects = (await multi.ReadAsync<SubjectMaster>()).ToList();

            IEnumerable<SchoolInfo> schoolDetails = Enumerable.Empty<SchoolInfo>();
            if (!multi.IsConsumed)
            {
                schoolDetails = (await multi.ReadAsync<SchoolInfo>()).ToList();
            }
            return (subjects, schoolDetails);
        }

    }
}
