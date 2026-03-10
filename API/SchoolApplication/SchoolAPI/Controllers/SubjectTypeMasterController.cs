using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using SchoolInfrastructure.Repositories;

namespace SchoolAPI.Controllers
{
    [Authorize]
    public class SubjectTypeMasterController : BaseApiController
    {
        private readonly ISubjectTypeMasterRepository _subjectTypeMasterRepository;

        public SubjectTypeMasterController(
            ISubjectTypeMasterRepository subjectTypeMasterRepository,
            ILogger<SubjectTypeMasterController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _subjectTypeMasterRepository = subjectTypeMasterRepository ?? throw new ArgumentNullException(nameof(subjectTypeMasterRepository));
        }

        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSubjectTypeMaster([FromBody] SubjectTypeMaster subjectTypeMaster)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeSubjectTypeMaster. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _subjectTypeMasterRepository.MergeSubjectTypeMasterAsync(subjectTypeMaster);
                _logger.LogInformation("Subject type merged successfully. SubjTypeId: {SubjTypeId}", subjectTypeMaster.SubjTypeId);
                return SuccessResponse(result, "Subject type master record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SubjectTypeMaster (SubjTypeId={SubjTypeId})", subjectTypeMaster?.SubjTypeId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument error in MergeSubjectTypeMaster");
                return ErrorResponse("Invalid request data.", 400, "INVALID_ARGUMENT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging SubjectTypeMaster (SubjTypeId={SubjTypeId})", subjectTypeMaster?.SubjTypeId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchSubjectTypeMaster()
        {
            try
            {
                var subjectTypes = await _subjectTypeMasterRepository.FetchSubjectTypeMasterAsync();

                _logger.LogInformation("Fetched {SubjectTypeCount} subject types", subjectTypes.Count());

                return SuccessResponse(new
                {
                    SubjectTypes = subjectTypes
                }, "Success");                      
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error fetching SubjectTypeMaster");
                return ErrorResponse("An error occurred while retrieving subject type master records.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching SubjectTypeMaster");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}
