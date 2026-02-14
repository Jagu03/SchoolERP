using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolInfrastructure.Repositories;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing class section subject mapping.
    /// </summary>
    [Authorize]
    public class ClassSectionSubjectMapController : BaseApiController
    {
        private readonly IClassSectionSubjectMapRepository _classSectionSubjectMapRepository;

        public ClassSectionSubjectMapController(
            IClassSectionSubjectMapRepository classSectionSubjectMapRepository,
            ILogger<ClassSectionSubjectMapController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _classSectionSubjectMapRepository = classSectionSubjectMapRepository ?? throw new ArgumentNullException(nameof(classSectionSubjectMapRepository));
        }

        /// <summary>
        /// Merge (insert/update) a class section subject map record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeClassSectionSubjectMap([FromBody] SchoolDomain.Entities.ClassSectionSubjectMap classSectionSubjectMap)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeClassSectionSubjectMap. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _classSectionSubjectMapRepository.MergeClassSectionSubjectMapAsync(classSectionSubjectMap);
                _logger.LogInformation("ClassSectionSubjectMap merged successfully. MapID: {MapId}", classSectionSubjectMap.Mapid);
                return SuccessResponse(result, "Class section subject map record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging ClassSectionSubjectMap (MapID={MapId})", classSectionSubjectMap?.Mapid);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging ClassSectionSubjectMap (MapID={MapId})", classSectionSubjectMap?.Mapid);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Fetch class section subject maps by filters.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchClassSectionSubjectMap([FromQuery] int classId, byte acadYearId, int? sectionId)
        {
            try
            {
                var data = await _classSectionSubjectMapRepository.FetchClassSectionSubjectMapAsync(classId, acadYearId, sectionId);

                _logger.LogInformation("Fetched {Count} allocations for ClassId={ClassId}, AcadYearId={AcadYearId}, SectionId={SectionId}",
                 data.classSectionSubjectMaps.Count(), classId, acadYearId, sectionId);

                return SuccessResponse(new
                {
                    ClassSectionSubjectMaps = data.classSectionSubjectMaps,
                    SchoolDetails = data.SchoolDetails
                }, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ClassSectionSubjectMap records.");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}




