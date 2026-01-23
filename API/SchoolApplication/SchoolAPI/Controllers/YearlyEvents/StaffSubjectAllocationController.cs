using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Interface.YearlyEvents;
using SchoolDomain.Entities.YearlyEvents;

namespace SchoolAPI.Controllers.YearlyEvents
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Default: Require authentication
    public class StaffSubjectAllocationController : ControllerBase
    {
        private readonly IStaffSubjectAllocationRepository _staffSubjectAllocationRepository;
        private readonly ILogger<StaffSubjectAllocationController> _logger;

        public StaffSubjectAllocationController(IStaffSubjectAllocationRepository staffSubjectAllocationRepository, ILogger<StaffSubjectAllocationController> logger)
        {
            _staffSubjectAllocationRepository = staffSubjectAllocationRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> MergeStaffSubjectAllocation([FromBody] StaffSubjectAllocation staffSubjectAllocation)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeStaffSubjectAllocation. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _staffSubjectAllocationRepository.MergeStaffSubjectAllocationAsync(staffSubjectAllocation);
                _logger.LogInformation("StaffSubjectAllocation merged successfully. AllocationId: {AllocationId}", staffSubjectAllocation.AllocationId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Staff subject allocation record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging StaffSubjectAllocation (AllocationId={AllocationId})", staffSubjectAllocation?.AllocationId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }

        [HttpGet("FetchStaffSubjectAllocation")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchStaffSubjectAllocation([FromQuery] byte acadYearId)
        {
            try
            {
                var result = await _staffSubjectAllocationRepository.FetchStaffSubjectAllocationAsync(acadYearId);
                return Ok(new DTOs.ApiResponseDto<IEnumerable<StaffSubjectAllocationview>>
                {
                    StatusCode = 200,
                    Message = "Staff subject allocation records fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching StaffSubjectAllocation for AcadYearId={AcadYearId}", acadYearId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }
    }
}