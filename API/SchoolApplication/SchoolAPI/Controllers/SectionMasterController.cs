using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing section master data.
    /// </summary>
    [Authorize]
    public class SectionMasterController : BaseApiController
    {
        private readonly ISectionMasterRepository _sectionMasterRepository;

        public SectionMasterController(
            ISectionMasterRepository sectionMasterRepository,
            ILogger<SectionMasterController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _sectionMasterRepository = sectionMasterRepository ?? throw new ArgumentNullException(nameof(sectionMasterRepository));
        }

        /// <summary>
        /// Merge (insert/update) a section master record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSectionMaster([FromBody] SectionMaster sectionMaster)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeSectionMaster. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _sectionMasterRepository.MergeSectionMasterAsync(sectionMaster);
                _logger.LogInformation("Section merged successfully. SectionID: {SectionId}", sectionMaster.SectionId);
                return SuccessResponse(result, "Section master record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SectionMaster (SectionID={SectionId})", sectionMaster?.SectionId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging SectionMaster (SectionID={SectionId})", sectionMaster?.SectionId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Fetch all section master records.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchSectionMaster()
        {
            try
            {
                var data = await _sectionMasterRepository.FetchSectionMasterAsync();

                _logger.LogInformation("Fetched {SectionCount} sections", data.sections.Count());

                return SuccessResponse(new
                {
                    SectionMaster = data.sections,
                    SchoolDetails = data.SchoolDetails
                }, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FetchSectionMaster");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}