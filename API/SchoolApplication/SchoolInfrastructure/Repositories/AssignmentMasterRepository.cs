using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolInfrastructure.Repositories
{
    public class AssignmentMasterRepository : IAssignmentMasterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<AssignmentMasterRepository> _logger;

        public AssignmentMasterRepository(IConfiguration configuration, ILogger<AssignmentMasterRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("LiveDBContext") ?? string.Empty;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'LiveDBContext' is not configured. Configure it in appsettings.json or an environment variable.");
            }
        }

        public async Task<string> MergeAssignmentMasterAsync(AssignmentMaster assignmentMaster)
        {
            await using var connection = new SqlConnection(_connectionString);
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EditId", assignmentMaster.EditId);
                parameters.Add("@AcadYearId", assignmentMaster.AcadYearId);
                parameters.Add("@ClassId", assignmentMaster.ClassId);
                parameters.Add("@SectionId", assignmentMaster.SectionId);
                parameters.Add("@SubjectId", assignmentMaster.SubjectId);
                parameters.Add("@StaffId", assignmentMaster.StaffId);
                parameters.Add("@AssignmentType", assignmentMaster.AssignmentType);
                parameters.Add("@Title", assignmentMaster.Title);
                parameters.Add("@TitleDescription", assignmentMaster.TitleDescription);
                parameters.Add("@GivenDate", assignmentMaster.GivenDate);
                parameters.Add("@DueDate", assignmentMaster.DueDate);
                parameters.Add("@MaxMarks", assignmentMaster.MaxMarks);
                parameters.Add("@Remarks", assignmentMaster.Remarks);
                parameters.Add("@CreatedUserId", assignmentMaster.CreatedUserId);
                parameters.Add("@LoginId", assignmentMaster.LoginId);
                parameters.Add("@Result", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);
                await connection.ExecuteAsync("SchoolAcad.MergeAssignment"
                    , parameters, commandType: System.Data.CommandType.StoredProcedure);
                return parameters.Get<string>("@Result") ?? string.Empty;
            }
        }   
    }
}