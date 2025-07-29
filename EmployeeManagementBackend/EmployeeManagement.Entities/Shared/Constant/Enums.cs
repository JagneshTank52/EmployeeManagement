using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities.Shared.Constant
{
    public class Enums
    {
        /// <summary>
        /// Enumeration for status codes.
        /// </summary>
        public enum EmpStatusCode
        {
            ModelStateError = -1,
            Ok = 200,
            Created = 201,
            BadRequest = 400,
            NotFound = 404, 
            ServerError = 500,
            UnAuthorized = 401,
            AccessDenied = 403,
            NotAllowed = 405,
            Conflict = 409
        }

        /// <summary>
        /// Enumeration for user caching times in minutes.
        /// </summary>
        //public enum UserCachingTime
        //{
        //    VeryShort = 2,
        //    SemiShort = 5,
        //    Short = 10,
        //    Medium = 30,
        //    Long = 60,
        //    SemiLong = 90,
        //    VeryLong = 180
        //}


        /// <summary>
        /// Enumeration for log types with descriptions.
        /// </summary>
        //public enum LogType
        //{
        //    [Description("Information")]
        //    Info = 1,
        //    [Description("Error")]
        //    Error = 2
        //}


        /// <summary>
        /// Enumeration for UserRoles
        /// </summary>
        //public enum UserRoles : short
        //{
        //    [Description("Super Admin")]
        //    SuperAdmin = 1,

        //    [Description("Organization Admin")]
        //    OrganizationAdmin = 2,
        //}

        /// <summary>
        /// Enumeration for permission to the roles.
        /// </summary>
        //public enum Permissions : int
        //{
        //    [Description("None")]
        //    None = 0,

        //    [Description("View")]
        //    View,

        //    [Description("Create")]
        //    Create,

        //    [Description("Edit")]
        //    Edit,

        //    [Description("Delete")]
        //    Delete,
        //}
    }
}
