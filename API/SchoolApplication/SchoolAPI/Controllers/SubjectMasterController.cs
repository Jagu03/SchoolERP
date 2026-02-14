using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing subject master data.
    /// </summary>
    [Authorize]
    public class SubjectMasterController : BaseApiController
    {
        private readonly ISubjectMasterRepository _subjectMasterRepository;

        public SubjectMasterController(
            ISubjectMasterRepository subjectMasterRepository,
            ILogger<SubjectMasterController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _subjectMasterRepository = subjectMasterRepository ?? throw new ArgumentNullException(nameof(subjectMasterRepository));
        }

        /// <summary>
        /// Merge (insert/update) a subject master record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSubjectMaster([FromBody] SubjectMaster subjectMaster)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeSubjectMaster. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _subjectMasterRepository.MergeSubjectMasterAsync(subjectMaster);
                _logger.LogInformation("Subject merged successfully. SubjectID: {SubjectId}", subjectMaster.SubjectId);
                return SuccessResponse(result, "Subject master record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SubjectMaster (SubjectID={SubjectId})", subjectMaster?.SubjectId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging SubjectMaster (SubjectID={SubjectId})", subjectMaster?.SubjectId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Fetch all subject master records.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchSubjectMaster()
        {
            try
            {
                var (subjects, schoolDetails) = await _subjectMasterRepository.FetchSubjectMasterAsync();

                _logger.LogInformation("Fetched {SubjectCount} subjects", subjects.Count());

                return SuccessResponse(new
                {
                    Subjects = subjects,
                    SchoolDetails = schoolDetails
                }, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subject master.");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}
