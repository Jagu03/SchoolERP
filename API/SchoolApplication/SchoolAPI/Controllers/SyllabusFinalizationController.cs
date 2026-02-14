using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing syllabus finalization.
    /// </summary>
    [Authorize]
    public class SyllabusFinalizationController : BaseApiController
    {
        private readonly ISyllabusFinalizationRepository _syllabusFinalizationRepository;

        public SyllabusFinalizationController(
            ISyllabusFinalizationRepository syllabusFinalizationRepository,
            ILogger<SyllabusFinalizationController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _syllabusFinalizationRepository = syllabusFinalizationRepository ?? throw new ArgumentNullException(nameof(syllabusFinalizationRepository));
        }

        /// <summary>
        /// Merge (insert/update) a syllabus finalization record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSyllabusFinalization([FromBody] SyllabusFinalization syllabusFinalization)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeSyllabusFinalization. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _syllabusFinalizationRepository.MergeSyllabusFinalizationAsync(syllabusFinalization);
                _logger.LogInformation("SyllabusFinalization merged successfully. SyllabusFinalId: {SyllabusFinalId}", syllabusFinalization.SyllabusFinalId);
                return SuccessResponse(result, "Syllabus finalization record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SyllabusFinalization (SyllabusFinalId={SyllabusFinalId})", syllabusFinalization?.SyllabusFinalId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging SyllabusFinalization (SyllabusFinalId={SyllabusFinalId})", syllabusFinalization?.SyllabusFinalId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Fetch syllabus finalization records by filters.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchSyllabusFinalization([FromQuery] int classId, short acadYearId, int? sectionId)
        {
            try
            {
                var data = await _syllabusFinalizationRepository.FetchSyllabusFinalizationAsync(classId, acadYearId, sectionId);

                return SuccessResponse(data, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching SyllabusFinalization records.");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}

