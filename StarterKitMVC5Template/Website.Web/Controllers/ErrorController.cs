using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using $safeprojectname$.Models;
using $safeprojectname$.Models.Account;

namespace $safeprojectname$.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Error/InternalServerError
        public ActionResult InternalServerError()
        {
            return View();
        }

        // GET: /Error/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        // GET: /Error/PageNotFound
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}