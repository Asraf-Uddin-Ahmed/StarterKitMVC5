using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class BaseController : Controller
    {
        private ILogger _logger;
        public BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //if (filterContext.ExceptionHandled)
            //    return;
            //filterContext.ExceptionHandled = true;
            _logger.Fatal(filterContext.Exception, "Fatal Error Occurred");
        }
    }
}
