using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Website.Foundation.Core.Identity;
using $safeprojectname$.Models;

namespace $safeprojectname$.Controllers
{
    public class BaseApiController : ApiController
    {

        private ModelFactory _modelFactory;

        protected ApplicationUserManager AppUserManager { get; private set; }
        protected ApplicationRoleManager AppRoleManager { get; private set; }

        public BaseApiController(ApplicationUserManager applicationUserManager, ApplicationRoleManager applicationRoleManager)
        {
            AppUserManager = applicationUserManager;
            AppRoleManager = applicationRoleManager;
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
