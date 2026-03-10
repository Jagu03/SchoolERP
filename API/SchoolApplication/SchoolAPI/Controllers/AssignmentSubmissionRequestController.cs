using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using SchoolInfrastructure;

namespace SchoolAPI.Controllers
{
    [Authorize]
    public class AssignmentSubmissionRequestController : BaseApiController
    {
        private readonly IAssignmentSubmissionRequestRepository _assignmentSubmissionRequestRepository;

        public AssignmentSubmissionRequestController(
            IAssignmentSubmissionRequestRepository assignmentSubmissionRequestRepository,
            ILogger<AssignmentSubmissionRequestController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _assignmentSubmissionRequestRepository = assignmentSubmissionRequestRepository ?? throw new ArgumentNullException(nameof(assignmentSubmissionRequestRepository));
            
        }

        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeAssignmentSubmissionRequest([FromBody] AssignmentSubmissionRequest assignmentSubmissionRequest)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeAssignmentSubmissionRequest. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _assignmentSubmissionRequestRepository.MergeAssignmentSubmissionRequestAsync(assignmentSubmissionRequest);
                _logger.LogInformation("Assignment submission request merged successfully. AssignmentId: {AssignmentId}", assignmentSubmissionRequest.AssignmentId);
                return SuccessResponse(result, "Assignment submission request processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging AssignmentSubmissionRequest (AssignmentId={AssignmentId})", assignmentSubmissionRequest?.AssignmentId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument error in MergeAssignmentSubmissionRequest");
                return ErrorResponse("Invalid request data.", 400, "INVALID_ARGUMENT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging AssignmentSubmissionRequest (AssignmentId={AssignmentId})", assignmentSubmissionRequest?.AssignmentId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}
