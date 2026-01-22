using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Default: Require authentication
    public class SectionMasterController : ControllerBase
    {
        private readonly ISectionMasterRepository _sectionMasterRepository;    
        private readonly ILogger<SectionMasterController> _logger;  

        public SectionMasterController(ISectionMasterRepository sectionMasterRepository, ILogger<SectionMasterController> logger)
        {
            _sectionMasterRepository = sectionMasterRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSectionMaster([FromBody] SchoolDomain.Entities.SectionMaster sectionMaster)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeSectionMaster. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _sectionMasterRepository.MergeSectionMasterAsync(sectionMaster);
                _logger.LogInformation("Section merged successfully. SectionID: {SectionId}", sectionMaster.SectionId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Section master record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SectionMaster (SectionID={SectionId})", sectionMaster?.SectionId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }

        [HttpGet("Fetch")]
        [AllowAnonymous] // Read-only operation, safe to allow anonymous access
        public async Task<IActionResult> FetchSectionMaster()
        {
            try
            {
                var data = await _sectionMasterRepository.FetchSectionMasterAsync();

                _logger.LogInformation("Fetched {SectionCount} sections", data.sections.Count());

                return Ok(new ApiResponseDto<object>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = new
                    {
                        SectionMaster = data.sections,
                        SchoolDetails = data.SchoolDetails
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FetchSectionMaster");
                throw; // Let GlobalExceptionMiddleware handle it
            }
        }
    }
}