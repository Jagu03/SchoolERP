using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolAPI.DTOs;

namespace SchoolAPI.Services
{
    /// <summary>
    /// Service for handling model state validation across the API.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Gets validation errors from ModelState.
        /// </summary>
        string GetValidationErrors(ModelStateDictionary modelState);
    }

    /// <summary>
    /// Implementation of validation service.
    /// </summary>
    public class ValidationService : IValidationService
    {
        public string GetValidationErrors(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return string.Empty;

            return string.Join("; ", modelState.Values
                .SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
