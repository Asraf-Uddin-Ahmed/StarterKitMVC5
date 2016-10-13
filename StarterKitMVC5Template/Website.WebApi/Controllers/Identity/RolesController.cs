using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Website.Identity.Constants.Roles;
using Website.Identity.Managers;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models;
using $safeprojectname$.Models.Request.Role;
using Website.Foundation.Core.Constant;
using Website.Foundation.Core.Aggregates.Identity;
using Ratul.Utility;
using $safeprojectname$.Codes.Core.Constant;
using $safeprojectname$.Codes.Core.Factories.Aggregates;

namespace $safeprojectname$.Controllers.Identity
{
    [Authorize(Roles = ApplicationRoles.ADMIN)]
    [RoutePrefix("roles")]
    public class RolesController : IdentityApiController
    {
        private IIdentityRoleResponseFactory _identityRoleResponseFactory;
        private ApplicationUserManager _applicationUserManager;
        private ApplicationRoleManager _applicationRoleManager;
        public RolesController(ILogger logger,
            IIdentityRoleResponseFactory identityRoleResponseFactory,
            ApplicationUserManager applicationUserManager,
            ApplicationRoleManager applicationRoleManager)
            : base(logger)
        {
            _identityRoleResponseFactory = identityRoleResponseFactory;
            _applicationUserManager = applicationUserManager;
            _applicationRoleManager = applicationRoleManager;
        }

        [Route("{id:guid}", Name = UriName.Identity.Roles.GET_ROLE)]
        public async Task<IHttpActionResult> GetRole(Guid Id)
        {
            var role = await _applicationRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(_identityRoleResponseFactory.Create(role));
            }

            return NotFound();

        }

        [Route("")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _applicationRoleManager.Roles;
            return Ok(_identityRoleResponseFactory.Create(roles.ToList()));
        }

        [Route("")]
        public async Task<IHttpActionResult> Create(CreateRoleRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomRole role = new CustomRole { Id = GuidUtility.GetNewSequentialGuid(), Name = model.Name };

            var result = await _applicationRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return CreatedAtRoute(UriName.Identity.Roles.GET_ROLE, new { id = role.Id }, _identityRoleResponseFactory.Create(role));

        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(Guid Id)
        {

            var role = await _applicationRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                IdentityResult result = await _applicationRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        [Route("ManageUsersInRole")]
        [HttpPost]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleRequestModel model)
        {
            var role = await _applicationRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (Guid user in model.EnrolledUsers)
            {
                var appUser = await _applicationUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!_applicationUserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await _applicationUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (Guid user in model.RemovedUsers)
            {
                var appUser = await _applicationUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await _applicationUserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("user/{userID:guid}", Name = UriName.Identity.Roles.GET_ROLE_BY_USER_ID)]
        public async Task<IHttpActionResult> GetRoleByUserID(Guid userID)
        {
            IList<string> roles = await _applicationUserManager.GetRolesAsync(userID);
            return Ok(roles);
        }
    }
}
