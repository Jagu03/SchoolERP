using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolApplication.Interface;
using SchoolInfrastructure.Repositories;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Default: Require authentication
    public class SyllabusFinalizationController : ControllerBase
    {
        private readonly ISyllabusFinalizationRepository _syllabusFinalizationRepository;
        private readonly ILogger<SyllabusFinalizationController> _logger;
        public SyllabusFinalizationController(ISyllabusFinalizationRepository syllabusFinalizationRepository, ILogger<SyllabusFinalizationController> logger)
        {
            _syllabusFinalizationRepository = syllabusFinalizationRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSyllabusFinalization([FromBody] SchoolDomain.Entities.SyllabusFinalization syllabusFinalization)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeSyllabusFinalization. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _syllabusFinalizationRepository.MergeSyllabusFinalizationAsync(syllabusFinalization);
                _logger.LogInformation("SyllabusFinalization merged successfully. SyllabusFinalId: {SyllabusFinalId}", syllabusFinalization.SyllabusFinalId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Syllabus finalization record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SyllabusFinalization (SyllabusFinalId={SyllabusFinalId})", syllabusFinalization?.SyllabusFinalId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }

        [HttpGet("FetchSyllabusFinalization")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchSyllabusFinalization([FromQuery] int classId, short acadYearId, int? sectionId)
        {
            try
            {
                //var data = await _syllabusFinalizationRepository.FetchSyllabusFinalizationAsync(classId, acadYearId, sectionId);

                //_logger.LogInformation("Fetched {AllocationCount} allocations for academic year {AcadYearId}",
                // data.syllabusFinalization.Count(), classId, acadYearId, sectionId);

                var data = await _syllabusFinalizationRepository.FetchSyllabusFinalizationAsync(classId, acadYearId, sectionId);

                //return Ok(new ApiResponseDto<object>
                //{
                //    StatusCode = 200,
                //    Message = "Success",
                //    Data = new
                //    {
                //        SyllabusFinalization = data.syllabusFinalization,
                //        SchoolDetails = data.SchoolDetails
                //    }
                //});
                return Ok(new ApiResponseDto<object>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ClassSectionSubjectMap records.");
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
