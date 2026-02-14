using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing assignment master data.
    /// </summary>
    [AllowAnonymous]
    public class AssignmentMasterController : BaseApiController
    {
        private readonly IAssignmentMasterRepository _assignmentMasterRepository;

        public AssignmentMasterController(
            IAssignmentMasterRepository assignmentMasterRepository,
            ILogger<AssignmentMasterController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _assignmentMasterRepository = assignmentMasterRepository ?? throw new ArgumentNullException(nameof(assignmentMasterRepository));
        }

        /// <summary>
        /// Merge (insert/update) an assignment master record.
        /// </summary>
        [HttpPost("merge")]
        public async Task<IActionResult> MergeAssignmentMaster([FromBody] AssignmentMaster assignmentMaster)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeAssignmentMaster. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _assignmentMasterRepository.MergeAssignmentMasterAsync(assignmentMaster);
                _logger.LogInformation("AssignmentMaster merged successfully. EditId: {EditId}", assignmentMaster.EditId);
                return SuccessResponse(result, "Assignment record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging AssignmentMaster (EditId={EditId})", assignmentMaster?.EditId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging AssignmentMaster (EditId={EditId})", assignmentMaster?.EditId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}

