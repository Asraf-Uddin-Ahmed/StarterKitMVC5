using Microsoft.AspNet.Identity;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Services;
using Website.Foundation.Core.Services.Email;
using Website.Identity.Managers;
using Website.Identity.Aggregates;
using Website.Identity.Providers;
using $safeprojectname$.Codes.Core.Factories;
using $safeprojectname$.Models;
using $safeprojectname$.Models.Request.Account;
using $safeprojectname$.Models.Request.Claim;
using Website.Identity.Constants.Roles;
using Microsoft.Owin.Security;
using $safeprojectname$.Configuration.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Website.Identity.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Website.Identity.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Owin.Security.OAuth;
using Website.Identity.Helpers;
using Website.Identity.Constants;
using System.Configuration;

namespace $safeprojectname$.Controllers.Identity
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : IdentityApiController
    {
        private ILogger _logger;
        private IApplicationUserResponseFactory _applicationUserResponseFactory;
        private ApplicationUserManager _applicationUserManager;
        private ApplicationRoleManager _applicationRoleManager;
        private IAuthRepository _authRepository;
        private IAuthHelper _authHelper;

        public AccountsController(ILogger logger,
            IAuthRepository authRepository,
            IAuthHelper authHelper,
            IApplicationUserResponseFactory applicationUserResponseFactory,
            ApplicationUserManager applicationUserManager, 
            ApplicationRoleManager applicationRoleManager)
        {
            _logger = logger;
            _authRepository = authRepository;
            _authHelper = authHelper;
            _applicationUserResponseFactory = applicationUserResponseFactory;
            _applicationUserManager = applicationUserManager;
            _applicationRoleManager = applicationRoleManager;
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_applicationUserResponseFactory.Create(_applicationUserManager.Users));
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await _applicationUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(_applicationUserResponseFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await _applicationUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(_applicationUserResponseFactory.Create(user));
            }

            return NotFound();

        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserRequestModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email
            };

            IdentityResult addUserResult = await _applicationUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            string code = await _applicationUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            await _applicationUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            return Created(locationHeader, _applicationUserResponseFactory.Create(user));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await _applicationUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _applicationUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await _applicationUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await _applicationUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(_applicationRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await _applicationUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await _applicationUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimRequestModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimRequestModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await _applicationUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await _applicationUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimRequestModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _applicationUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimRequestModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await _applicationUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }

    }
}
