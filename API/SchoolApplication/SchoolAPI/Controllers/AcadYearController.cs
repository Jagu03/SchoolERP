using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;
using SchoolInfrastructure.Repositories;

namespace SchoolAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AcadYearController : BaseApiController
    {
        private readonly IAcadYearRepository _acadYearRepository;

        public AcadYearController(
           IAcadYearRepository acadYearRepository,
           ILogger<AcadYearController> logger,
           IValidationService validationService)
           : base(logger, validationService)
        {
            _acadYearRepository = acadYearRepository ?? throw new ArgumentNullException(nameof(acadYearRepository));
        }

        /// <summary>
        /// Merge (insert/update) a class master record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeAcadYearAsync([FromBody] AcadYear acadYear)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeAcadYear. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _acadYearRepository.MergeAcadYearAsync(acadYear);

                _logger.LogInformation("AcadYear merged successfully. AcadYearID: {AcadYearId}", acadYear.AcadYearId);
                return SuccessResponse(result, "AcadYear record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging AcadYear (AcadYearID={AcadYearId})", acadYear?.AcadYearId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument error in MergeAcadYear");
                return ErrorResponse("Invalid request data.", 400, "INVALID_ARGUMENT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging AcadYear (AcadYearID={AcadYearId})", acadYear?.AcadYearId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }


        /// <summary>
        /// Fetch all academic year records.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchAcadYear()
        {
            try
            {
                var (acadYears, schoolDetails) = await _acadYearRepository.FetchAllAcadYearAsync();
                _logger.LogInformation("Fetched {AcadYearCount} academic years", acadYears.Count());

                return SuccessResponse(new
                {
                    AcadYear = acadYears,
                    SchoolDetails = schoolDetails
                }, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching academic year.");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}

