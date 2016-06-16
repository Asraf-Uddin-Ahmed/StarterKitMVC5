using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers.Resource
{
    public class HomeController : BaseApiController
    {
        // GET: api/Home
        public string Get()
        {
            return "API is running...";
        }

    }
}
