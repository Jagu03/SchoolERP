using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Interface.YearlyEvents;
using SchoolDomain.Entities.YearlyEvents;

namespace SchoolAPI.Controllers.YearlyEvents
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LessonPlanController : ControllerBase
    {
      private readonly ILessonPlanRepository _lessonPlanRepository;
      private readonly ILogger<LessonPlanController> _logger;

      public LessonPlanController(ILessonPlanRepository lessonPlanRepository, ILogger<LessonPlanController> logger)
      {
          _lessonPlanRepository = lessonPlanRepository;
          _logger = logger;
      }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> MergeLessonPlan([FromBody] LessonPlanMaster lessonPlanMaster)
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
                var result = await _lessonPlanRepository.MergeLessonPlanAsync(lessonPlanMaster);
                _logger.LogInformation("LessonPlan merged successfully. EditId: {EditId}", lessonPlanMaster.EditId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Lesson plan record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging LessonPlan (EditId={EditId})", lessonPlanMaster?.EditId);
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
        public async Task<IActionResult> FetchLessonPlan(int ClassId, int subjectid, short acadYearId)
        {
            try
            { 
                var result = await _lessonPlanRepository.FetchLessonPlanAsync(ClassId, subjectid, acadYearId);
                return Ok(new DTOs.ApiResponseDto<IEnumerable<LessonPlanMasterView>>
                {
                    StatusCode = 200,
                    Message = "Lesson plans fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lesson plans for ClassId={ClassId}, SubjectId={SubjectId}, AcadYearId={AcadYearId}", ClassId, subjectid, acadYearId);
                return StatusCode(500, new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching lesson plans.",
                    Data = null
                });
            }
        }
    }
}