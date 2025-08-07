using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EmployeeManagement.Entities.Shared.Convertor
{
    public static class ValidationConvertor
    {
        public static IActionResult CreateValidationErrorResponse(ActionContext context, bool isDev)
        {
            var validationErrors = ConvertModelStateToValidationDetails(context.ModelState);

            var errorResponse = new ErrorResponse(
                errors: validationErrors,
                message: Messages.Error.General.validationError,
                statusCode: 400
            );

            return new BadRequestObjectResult(errorResponse);
        }

        public static List<ValidationDetailModel> ConvertModelStateToValidationDetails(ModelStateDictionary modelState)
        {
            var validationErrors = new List<ValidationDetailModel>();

            foreach (var kvp in modelState)
            {
                var propertyName = kvp.Key;
                var errors = kvp.Value?.Errors;

                if (errors != null && errors.Count > 0)
                {
                    foreach (var error in errors)
                    {
                        var inputName = string.IsNullOrWhiteSpace(propertyName) ? "RequestBody" : propertyName;

                        validationErrors.Add(new ValidationDetailModel
                        {
                            InputName = inputName,
                            ValidationMessage = error.ErrorMessage
                        });
                    }
                }
            }
            return validationErrors;
        }
    }
}
