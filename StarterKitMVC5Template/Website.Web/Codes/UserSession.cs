using Ratul.Mvc;
using Ratul.Mvc.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Codes
{
    public static class UserSession
    {
        private enum SessionKeys
        {
            CurrentUser,
            ActionResponseMessage,
            UserTimeZoneOffsetInMinute
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public static UserIdentity CurrentUser
        {
            get
            {
                UserIdentity identity = (UserIdentity)HttpContext.Current.Session[SessionKeys.CurrentUser.ToString()];
                if (identity == null && HttpContext.Current.Request.IsAuthenticated)
                {
                    identity = (UserIdentity)HttpContext.Current.Session[SessionKeys.CurrentUser.ToString()];
                }
                return identity;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.CurrentUser.ToString()] = value;
            }
        }

        public static ActionResponse ActionResponseMessage
        {
            get
            {
                ActionResponse result = (ActionResponse)HttpContext.Current.Session[SessionKeys.ActionResponseMessage.ToString()];
                HttpContext.Current.Session[SessionKeys.ActionResponseMessage.ToString()] = null;
                return result;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.ActionResponseMessage.ToString()] = value;
            }
        }

        public static int UserTimeZoneOffsetInMinute
        {
            get
            {
                return (int)HttpContext.Current.Session[SessionKeys.UserTimeZoneOffsetInMinute.ToString()];
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.UserTimeZoneOffsetInMinute.ToString()] = value;
            }
        }
    }
}