using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Shared.Constant
{
    public class Messages
    {
        /// <summary>
        /// Success messages
        /// </summary>
        public static class Success
        {
            /// <summary>
            /// General success messages
            /// </summary>
            public static class General
            {
                public const string Success = "Success";
            }
        }

        public static class Error
        {
            public static class Exception
            {
                public const string InternalServerErrorMessage = "An Error has occurred, and we are working to fix the problem! We will be up and running shortly";
                public const string UnauthorizedErrorMessage = "You are not authorized to perform this action. Please contact your administrator.";
                public const string BadRequestErrorMessage = "We couldn't process your request. Please try again later";
                public const string DataValidationErrorMessage = "One or more validation error occurred.";
                public const string DataNotFoundExceptionMessage = "Data not found.";
                public const string DataConflictExceptionMessage = "Data conflict exception occurred.";
                public const string InvalidOperationExceptionMessage = "Invalid operation exception occurred.";
            }

            public static class General
            {
                public static readonly Func<string, string> NotFoundMessage = (entityName) => $"{entityName} not found.";
                public static readonly Func<string, string> ConflictMessage = (entityName) => $"{entityName} with same name already present.";
                public static readonly Func<string, string[], string> DeleteConflictDueToRelationsMessage = (parentEntity, childEntities) =>
                {
                    if (childEntities == null || childEntities.Length == 0)
                        return $"Cannot delete {parentEntity} due to existing related entities.";

                    string relatedEntities = string.Join(" or ", childEntities);
                    return $"Cannot delete {parentEntity} because it has related {relatedEntities}.";
                };
                public static readonly Func<string, string> AddError = (entityName) => $"Error occurred while adding {entityName}.";
                public static readonly Func<string, string> UpdateError = (entityName) => $"Error occurred while updating {entityName}.";
                public static readonly Func<string, string> DeleteError = (entityName) => $"Error occurred while deleting {entityName}.";
                public static readonly Func<string, string> NotExists = (entityName) => $"The given {entityName} does not exist. Please verify the {entityName} and try again.";
                public const string InvalidEmailMessage = "Invalid email. Please enter a valid one.";
                public static readonly Func<string, string> RequiredFieldMessage = (fieldName) => $"{fieldName} is required.";
                public static readonly Func<string, int, string> MaxLengthExceededMessage = (fieldName, maxLength) => $"{fieldName} must not exceed {maxLength} characters.";
                public static readonly Func<string, string> AlreadyExistMessage = (entityName) => $"{entityName} is already exist.";
                public static readonly Func<string, string, string, string> AlreadyExistsWithAttributeMessage = (entityName, attribute, value) => $"{entityName} with this {attribute}({value}) already exists.";
                public static readonly Func<string, int, string, string> HttpClientRequestFailedMessage = (url, statusCode, message) => $"Request to '{url}' failed with status code {statusCode}: {message}";
                public static readonly Func<string, string[], string> InvalidFileTypeMessage = (fieldName, allowedExtensions) => $"{fieldName} must be a valid file of type: {string.Join(", ", allowedExtensions)}.";
                public static readonly Func<string, long, string> MaxFileSizeExceededMessage = (fieldName, maxSizeBytes) =>
                {
                    long maxSizeMb = maxSizeBytes / (1024 * 1024);
                    return $"{fieldName} must not exceed {maxSizeMb} MB.";
                };
                public static readonly Func<string, string[], string> InvalidEnumValueMessage = (fieldName, allowedValues) => $"{fieldName} must be one of the following values: {string.Join(", ", allowedValues)}.";
                public const string InvalidDeserializationMessage = "Response content is null or cannot be deserialized.";
                public const string EmptyHubName = "Hub name is empty or not provided.";
            }

            /// <summary>
            /// Auth service related error messages
            /// </summary>
            public static class Auth
            {
                public const string InvalidPasswordMessage = "Incorrect credential. Please try again.";
                public const string TokenExpired = "Token expired";
                public const string InvalidJwtRefreshToken = "Invalid or expired refresh token.";
                public static readonly Func<string, string> MissingClaimMessage = (claimName) => $"{claimName} claim is missing in the token.";
            }

            /// <summary>
            /// Organization service related error messages
            /// </summary>
            //public static class Org
            //{
            //    public static readonly Func<string, string> DatabaseCreationErrorMessage = (OrganizationName) => $"Unexpected error when creating organization database for {OrganizationName}";
            //}
        }

    }
}
