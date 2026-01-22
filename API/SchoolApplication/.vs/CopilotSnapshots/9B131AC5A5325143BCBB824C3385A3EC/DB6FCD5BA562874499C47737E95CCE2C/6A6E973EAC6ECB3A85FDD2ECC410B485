using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolApplication.Interface;
using SchoolInfrastructure.Repositories;
using System.Collections;
using static System.Collections.Specialized.BitVector32;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Default: Require authentication
    public class ClassSectionSubjectMapController : ControllerBase
    {
        private readonly IClassSectionSubjectMapRepository _classSectionSubjectMapRepository;
        private readonly ILogger<ClassSectionSubjectMapController> _logger;

        public ClassSectionSubjectMapController(IClassSectionSubjectMapRepository classSectionSubjectMapRepository, ILogger<ClassSectionSubjectMapController> logger)
        {
            _classSectionSubjectMapRepository = classSectionSubjectMapRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> MergeClassSectionSubjectMap([FromBody] SchoolDomain.Entities.ClassSectionSubjectMap classSectionSubjectMap)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeClassSectionSubjectMap. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _classSectionSubjectMapRepository.MergeClassSectionSubjectMapAsync(classSectionSubjectMap);
                _logger.LogInformation("ClassSectionSubjectMap merged successfully. MapID: {MapId}", classSectionSubjectMap.Mapid);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Class section subject map record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging ClassSectionSubjectMap (MapID={MapId})", classSectionSubjectMap?.Mapid);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }

        [HttpGet("FetchClassSectionSubjectMap")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchClassSectionSubjectMap([FromQuery] int classId, short acadYearId, int? sectionId)
        {
            try
            {
                var data = await _classSectionSubjectMapRepository.FetchClassSectionSubjectMapAsync(classId, acadYearId, sectionId);

                _logger.LogInformation("Fetched {AllocationCount} allocations for academic year {AcadYearId}",
                 data.classSectionSubjectMaps.Count(), classId, acadYearId, sectionId);

                return Ok(new ApiResponseDto<object>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = new
                    {
                        ClassSectionSubjectMaps = data.classSectionSubjectMaps,
                        SchoolDetails = data.SchoolDetails
                    }
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



