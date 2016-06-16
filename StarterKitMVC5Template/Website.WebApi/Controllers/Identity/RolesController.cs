using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Website.Foundation.Core.Identity;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models;
using $safeprojectname$.Models.Request.Role;

namespace $safeprojectname$.Controllers.Identity
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : IdentityApiController
    {
        private IIdentityRoleResponseFactory _identityRoleResponseFactory;
        private ApplicationUserManager _applicationUserManager;
        private ApplicationRoleManager _applicationRoleManager;
        public RolesController(IIdentityRoleResponseFactory identityRoleResponseFactory, 
            ApplicationUserManager applicationUserManager, 
            ApplicationRoleManager applicationRoleManager)
        {
            _identityRoleResponseFactory = identityRoleResponseFactory;
            _applicationUserManager = applicationUserManager;
            _applicationRoleManager = applicationRoleManager;
        }

        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string Id)
        {
            var role = await _applicationRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(_identityRoleResponseFactory.Create(role));
            }

            return NotFound();

        }

        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _applicationRoleManager.Roles;
            return Ok(_identityRoleResponseFactory.Create(roles));
        }

        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var result = await _applicationRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, _identityRoleResponseFactory.Create(role));

        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string Id)
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
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleRequestModel model)
        {
            var role = await _applicationRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
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

            foreach (string user in model.RemovedUsers)
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
    }
}
