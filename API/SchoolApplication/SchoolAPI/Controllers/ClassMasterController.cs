using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Services;
using SchoolApplication.Interface;
using SchoolDomain.Entities;

namespace SchoolAPI.Controllers
{
    /// <summary>
    /// Controller for managing class master data.
    /// </summary>
    [Authorize]
    public class ClassMasterController : BaseApiController
    {
        private readonly IClassMasterRepository _classMasterRepository;

        public ClassMasterController(
            IClassMasterRepository classMasterRepository,
            ILogger<ClassMasterController> logger,
            IValidationService validationService)
            : base(logger, validationService)
        {
            _classMasterRepository = classMasterRepository ?? throw new ArgumentNullException(nameof(classMasterRepository));
        }

        /// <summary>
        /// Merge (insert/update) a class master record.
        /// </summary>
        [HttpPost("merge")]
        [AllowAnonymous]
        public async Task<IActionResult> MergeClassMaster([FromBody] ClassMaster classMaster)
        {
            if (!ValidateModel(out var errors))
            {
                _logger.LogWarning("Invalid model state for MergeClassMaster. Errors: {Errors}", errors);
                return ValidationErrorResponse(errors);
            }

            try
            {
                var result = await _classMasterRepository.MergeClassMasterAsync(classMaster);

                _logger.LogInformation("Class merged successfully. ClassID: {ClassId}", classMaster.ClassId);

                return SuccessResponse(result, "Class master record processed successfully.", 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error merging ClassMaster (ClassID={ClassId})", classMaster?.ClassId);
                return ErrorResponse("An error occurred while processing your request.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument error in MergeClassMaster");
                return ErrorResponse("Invalid request data.", 400, "INVALID_ARGUMENT");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error merging ClassMaster (ClassID={ClassId})", classMaster?.ClassId);
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }

        /// <summary>
        /// Fetch all class master records.
        /// </summary>
        [HttpGet("fetch")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchClassMaster()
        {
            try
            {
                var data = await _classMasterRepository.FetchClassMasterAsync();

                _logger.LogInformation("Fetched {ClassCount} classes", data.classes.Count());

                return SuccessResponse(new
                {
                    ClassMaster = data.classes,
                    SchoolInfo = data.School
                }, "Success");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic error fetching ClassMaster");
                return ErrorResponse("An error occurred while retrieving class master records.", 500, "BUSINESS_LOGIC_ERROR");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching ClassMaster");
                return ErrorResponse("An unexpected error occurred.", 500, "INTERNAL_SERVER_ERROR");
            }
        }
    }
}

