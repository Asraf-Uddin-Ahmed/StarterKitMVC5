using log4net;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace $safeprojectname$.Configuration
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static readonly ILog _log4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void Log(ExceptionLoggerContext context)
        {
            _log4Net.Fatal(String.Format("Unhandled exception thrown in {0} for request {1}", context.Request.Method, context.Request.RequestUri), context.Exception);
        }
    }
}