using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing class section allocations.
    /// </summary>
    [Authorize]
    public class ClassSectionAllocationController : BaseApiController
    {
        private readonly IClassSectionAllocationRepository _classSectionAllocationRepository;

        public ClassSectionAllocationController(
            IClassSectionAllocationRepository classSectionAllocationRepository,
            ILogger<ClassSectionAllocationController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _classSectionAllocationRepository = classSectionAllocationRepository ?? throw new ArgumentNullException(nameof(classSectionAllocationRepository));
        }

        /// <summary>
        /// Merge (insert/update) a class section allocation record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeAllocateClassSection([FromBody] ClassSectionAllocation classSectionAllocation)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeAllocateClassSection. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _classSectionAllocationRepository.MergeAllocateClassSectionAsync(classSectionAllocation);

                _logger.LogInformation("Class section allocation merged successfully. AllocationID: {AllocationId}", classSectionAllocation.AllocationId);

                return SuccessResponse(result, "Class section allocation processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error merging ClassSectionAllocation (AllocationID={AllocationId})", classSectionAllocation?.AllocationId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument error in MergeAllocateClassSection");
                return ErrorResponse("Invalid request data.", 400, "INVALID_ARGUMENT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging ClassSectionAllocation (AllocationID={AllocationId})", classSectionAllocation?.AllocationId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Fetch class section allocations for a given academic year.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchClassSectionAllocations([FromQuery] byte acadYearId)
        {
            if (acadYearId <= 0)
            {
                _logger.LogWarning("Invalid AcadYearId provided: {AcadYearId}", acadYearId);
                return ErrorResponse("AcadYearId query parameter is required and must be greater than 0.", 400, "INVALID_PARAMETER");
            }

            try
            {
                var data = await _classSectionAllocationRepository.FetchClassSectionAllocationsAsync(acadYearId);

                _logger.LogInformation("Fetched {AllocationCount} allocations for academic year {AcadYearId}",
                    data.allocations.Count(), acadYearId);

                return SuccessResponse(new
                {
                    ClassSectionAllocations = data.allocations,
                    SchoolDetails = data.SchoolDetails
                }, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ClassSectionAllocations for academic year {AcadYearId}", acadYearId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}