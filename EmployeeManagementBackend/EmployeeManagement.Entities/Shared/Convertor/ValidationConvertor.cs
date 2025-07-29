using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeManagement.Entities.Shared.Convertor
{
    /// <summary>
    /// Converter for transforming ModelState validation errors to custom validation format
    /// </summary>

    public static class ValidationConvertor
    {

        /// <summary>
        /// Converts ModelState errors to ValidationDetailModel list
        /// </summary>
        /// <param name="modelState">The ModelState containing validation errors</param>
        /// <returns>List of ValidationDetailModel objects</returns>
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
                        validationErrors.Add(new ValidationDetailModel
                        {
                            InputName = propertyName,
                            ValidationMessage = error.ErrorMessage
                        });
                    }
                }
            }

            return validationErrors;
        }

        //Here some mistake check it
        /// <summary>
        /// Creates a DataValidationException from ModelState errors
        /// </summary>
        /// <param name="modelState">The ModelState containing validation errors</param>
        /// <param name="message">Optional custom message</param>
        /// <returns>DataValidationException with converted validation errors</returns>
        public static DataValidationException CreateValidationException(ModelStateDictionary modelState, string message = "Validation failed")
        {
            var validationDetails = ConvertModelStateToValidationDetails(modelState);

            // Create mock ValidationFailure objects for DataValidationException constructor
            var validationFailures = validationDetails.Select(v =>
                new MockValidationFailure(v.InputName!, v.ValidationMessage!)
            );

            return new DataValidationException(validationDetails, message);
        }

        /// <summary>
        /// Checks if ModelState has any validation errors
        /// </summary>
        /// <param name="modelState">The ModelState to check</param>
        /// <returns>True if there are validation errors, false otherwise</returns>
        public static bool HasValidationErrors(ModelStateDictionary modelState)
        {
            return !modelState.IsValid;
        }

        /// <summary>
        /// Mock ValidationFailure class to work with DataValidationException
        /// Since you're not using FluentValidation, we create a simple implementation
        /// </summary>
        internal class MockValidationFailure
        {
            public string PropertyName { get; }
            public string ErrorMessage { get; }

            public MockValidationFailure(string propertyName, string errorMessage)
            {
                PropertyName = propertyName;
                ErrorMessage = errorMessage;
            }
        }
    }
}
