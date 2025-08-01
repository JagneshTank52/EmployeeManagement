using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Shared.Constant
{
    public class Messages
    {
        public static class Success
        {
            public static class General
            {
                public const string Success = "Success";
                public static readonly Func<string, string> AuthSuccess = (entityName) => $"{entityName} successfully.";
                public static readonly Func<string, string> GetSuccess = (entityName) => $"{entityName} fetched successfully.";
            }
        }

        public static class Error
        {
            public static class Exception
            {
                public const string InternalServerErrorMessage = "An Error has occurred, and we are working to fix the problem! We will be up and running shortly";
                public const string UnauthorizedErrorMessage = "You are not authorized to perform this action. Please contact your administrator.";
                public const string ForbiddenAccessExceptionMessage = "You do not have permission to access this resource.";
                public const string BadRequestErrorMessage = "We couldn't process your request. Please try again later";
                public const string DataValidationErrorMessage = "One or more validation error occurred.";
                public const string DataNotFoundExceptionMessage = "Data not found.";
                public const string DataConflictExceptionMessage = "Data conflict exception occurred.";
                public const string InvalidOperationExceptionMessage = "Invalid operation exception occurred.";
            }

            public static class General
            {
                public static string PermissionNotAssignedMessage = "Permission not assigned to this role.";
                public static readonly Func<string, string, string> ForbiddenPermissionMessage =
                    (permissionType, permission) => $"You do not have {permissionType} permission for {permission}.";
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
                public static readonly Func<string, string> AlreadyExistMessage = (entityName) => $"{entityName} is already exist.";
                public static readonly Func<string, string, string, string> AlreadyExistsWithAttributeMessage = (entityName, attribute, value) => $"{entityName} with this {attribute}({value}) already exists.";
                public static readonly Func<string, int, string, string> HttpClientRequestFailedMessage = (url, statusCode, message) => $"Request to '{url}' failed with status code {statusCode}: {message}";
                public static readonly Func<string, string[], string> InvalidEnumValueMessage = (fieldName, allowedValues) => $"{fieldName} must be one of the following values: {string.Join(", ", allowedValues)}.";
                public const string InvalidDeserializationMessage = "Response content is null or cannot be deserialized.";
                public const string validationError = "Validation faild";

            }

            public static class Auth
            {
                public const string InvalidPasswordMessage = "Incorrect credential. Please try again.";
                public const string TokenExpired = "Token expired";
                public const string InvalidJwtRefreshToken = "Invalid or expired refresh token.";
                public static readonly Func<string, string> MissingClaimMessage = (claimName) => $"{claimName} claim is missing in the token.";
            }
        }

    }
}
