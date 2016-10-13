using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Codes.Core.Constant
{
    public static class UriName
    {
        public static class Resource
        {
            
        }
        
        public static class Identity
        {
            public static class Accounts
            {
                public const string GET_USER = "GetUserById";
                public const string CONFIRM_EMAIL = "ConfirmEmailRoute";
                public const string RESET_PASSWORD = "ResetPasswordRoute";
            }
            public static class Roles
            {
                public const string GET_ROLE_BY_USER_ID = "GetRoleByUserID";
                public const string GET_ROLE = "GetRoleById";
            }
            public static class Claims
            {
                public const string GET_CLAIM_BY_USER_ID = "GetClaimByUserID";
            }
            
        }
    }
}