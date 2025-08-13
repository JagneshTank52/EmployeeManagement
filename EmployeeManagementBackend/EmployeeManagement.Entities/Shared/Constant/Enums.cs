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

        public enum DropDownType
        {
            Technology = 1,
            TaskStatus = 2,
            Employee = 3,
        }


        public enum StatusColor
        {
            [Description("#00FF00")]
            New = 1,
            [Description("#00FF00")]
            InProgress = 2,
            [Description("#00FF00")]
            Completed = 3,
            [Description("#00FF00")]
            Blocked = 4
        }

        /// <summary>
        /// Enumeration for UserRoles
        /// </summary>
        //public enum UserRoles : short
        //{
        //    [Description("Admin")]
        //    SuperAdmin = 1,

        //    [Description("User")]
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

        public enum Permission
        {
            Department,
            Project,
            Attendance,
            Employee
        }

        public enum PermissionType
        {
            Read,
            Write,
            Delete
        }
    }
}
