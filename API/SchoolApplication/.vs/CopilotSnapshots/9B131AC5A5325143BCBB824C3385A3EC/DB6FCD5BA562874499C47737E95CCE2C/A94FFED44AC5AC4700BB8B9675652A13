using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolApplication.Interface;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Default: Require authentication
    public class SubjectMasterController : ControllerBase
    {
       private readonly ISubjectMasterRepository _subjectMasterRepository;
       private readonly ILogger<SubjectMasterController> _logger;

       public SubjectMasterController(ISubjectMasterRepository subjectMasterRepository, ILogger<SubjectMasterController> logger)
       {
           _subjectMasterRepository = subjectMasterRepository;
           _logger = logger;
       }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> MergeSubjectMaster([FromBody] SchoolDomain.Entities.SubjectMaster subjectMaster)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeSubjectMaster. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _subjectMasterRepository.MergeSubjectMasterAsync(subjectMaster);
                _logger.LogInformation("Subject merged successfully. SubjectID: {SubjectId}", subjectMaster.SubjectId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Subject master record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging SubjectMaster (SubjectID={SubjectId})", subjectMaster?.SubjectId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }

        [HttpGet("FetchSubjectMaster")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchSubjectMaster()
        {
            try
            {
                var (subjects, schoolDetails) = await _subjectMasterRepository.FetchSubjectMasterAsync();

                _logger.LogInformation("Fetched {SubjectCount} subjects", subjects.Count());

                var response = new
                {
                    Subjects = subjects,
                    SchoolDetails = schoolDetails
                };

                return Ok(new ApiResponseDto<object>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subject master.");
                throw; // Let GlobalExceptionMiddleware handle it
            }
        }
    }
}
