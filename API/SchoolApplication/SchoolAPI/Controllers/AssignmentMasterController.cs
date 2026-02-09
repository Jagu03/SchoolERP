using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Interface;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AssignmentMasterController : ControllerBase
    {
       private readonly IAssignmentMasterRepository _assignmentMasterRepository;
       private readonly ILogger<AssignmentMasterController> _logger;

       public AssignmentMasterController(IAssignmentMasterRepository assignmentMasterRepository, ILogger<AssignmentMasterController> logger)
       {
           _assignmentMasterRepository = assignmentMasterRepository;
           _logger = logger;
       }
        [HttpPost]
        public async Task<IActionResult> MergeAssignmentMaster([FromBody] SchoolDomain.Entities.AssignmentMaster assignmentMaster)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                _logger.LogWarning("Invalid model state for MergeAssignmentMaster. Errors: {Errors}", errors);
                return BadRequest(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 400,
                    Message = "Model validation failed.",
                    Data = errors
                });
            }
            try
            {
                var result = await _assignmentMasterRepository.MergeAssignmentMasterAsync(assignmentMaster);
                _logger.LogInformation("AssignmentMaster merged successfully. EditId: {EditId}", assignmentMaster.EditId);
                return Ok(new DTOs.ApiResponseDto<string>
                {
                    StatusCode = 200,
                    Message = "Assignment record processed successfully.",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging AssignmentMaster (EditId={EditId})", assignmentMaster?.EditId);
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
