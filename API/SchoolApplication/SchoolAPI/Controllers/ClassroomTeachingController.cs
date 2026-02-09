using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Interface;
using SchoolDomain.Entities.YearlyEvents;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ClassroomTeachingController : ControllerBase
    {
        private readonly SchoolApplication.Interface.IClassroomTeachingRepository _classroomTeachingRepository;
        private readonly ILogger<ClassroomTeachingController> _logger;
        public ClassroomTeachingController(IClassroomTeachingRepository classroomTeachingRepository, ILogger<ClassroomTeachingController> logger)
        {
            _classroomTeachingRepository = classroomTeachingRepository;
            _logger = logger;
        }
        [HttpPost("MergeClassroomTeaching")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeClassroomTeachingAsync(SchoolDomain.Entities.ClassroomTeaching classroomTeaching)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeLessonPlan. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _classroomTeachingRepository.MergeClassroomTeachingAsync(classroomTeaching);
                _logger.LogInformation("ClassroomTeaching merged successfully. EditId: {EditId}", classroomTeaching.EditId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Classroom teaching record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging ClassroomTeaching (EditId={EditId})", classroomTeaching?.EditId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetClassroomTeachingByDateAsync([FromQuery] short acadYearId, [FromQuery] int classId, [FromQuery] int subjectId, [FromQuery] DateTime fromdate, [FromQuery] DateTime todate)
        {
            try
            {
                var result = await _classroomTeachingRepository.GetClassroomTeachingByDateAsync(acadYearId, classId, subjectId, fromdate, todate);
                _logger.LogInformation("Fetched ClassroomTeaching records for ClassId: {ClassId}, SubjectId: {SubjectId} from {FromDate} to {ToDate}", classId, subjectId, fromdate, todate);
                return Ok(new DTOs.ApiResponseDto<IEnumerable<SchoolDomain.Entities.ClassroomTeachingview>>
                {
                    StatusCode = 200,
                    Message = "Classroom teaching records fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ClassroomTeaching records for ClassId: {ClassId}, SubjectId: {SubjectId} from {FromDate} to {ToDate}", classId, subjectId, fromdate, todate);
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