using Ninject;
using Ninject.Extensions.Logging;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class HomeController : BaseController
    {
        private ILogger _logger;
        [Inject]
        public HomeController(ILogger logger)
            : base(logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}