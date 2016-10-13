using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Aggregates.Identity;
using Website.Foundation.Core.Constant;
using Website.Foundation.Core.Factories;
using Website.Foundation.Core.Services;
using Website.Identity.Helpers;
using Website.Identity.Managers;
using Website.Identity.Providers;
using Website.Identity.Repositories;
using $safeprojectname$.Codes.Core.Constant;
using $safeprojectname$.Codes.Core.Factories.Aggregates;
using $safeprojectname$.Models.Request.Account;
using $safeprojectname$.Models.Request.Claim;
using $safeprojectname$.Models.Response.Aggregates;
using Microsoft.AspNet.Identity;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.Controllers.Identity
{
    [RoutePrefix("accounts")]
    public class AccountsController : IdentityApiController
    {
        private ILogger _logger;
        private IApplicationUserResponseFactory _applicationUserResponseFactory;
        private ApplicationUserManager _applicationUserManager;
        private ApplicationRoleManager _applicationRoleManager;
        private IAuthRepository _authRepository;
        private IAuthHelper _authHelper;
        private IApplicationUserFactory _applicationUserFactory;
        public AccountsController(ILogger logger,
            IAuthRepository authRepository,
            IAuthHelper authHelper,
            IApplicationUserResponseFactory applicationUserResponseFactory,
            ApplicationUserManager applicationUserManager,
            IApplicationUserFactory applicationUserFactory,
            ApplicationRoleManager applicationRoleManager
            )
            : base(logger)
        {
            _logger = logger;
            _authRepository = authRepository;
            _authHelper = authHelper;
            _applicationUserResponseFactory = applicationUserResponseFactory;
            _applicationUserManager = applicationUserManager;
            _applicationRoleManager = applicationRoleManager;
            _applicationUserFactory = applicationUserFactory;
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                return Ok(_applicationUserResponseFactory.Create(_applicationUserManager.Users.ToList()));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to GetUsers");
                return InternalServerError(ex, "Failed to GetUsers");
            }
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}", Name = UriName.Identity.Accounts.GET_USER)]
        public async Task<IHttpActionResult> GetUser(Guid Id)
        {
            try
            {
                var user = await _applicationUserManager.FindByIdAsync(Id);
                if (user != null)
                {
                    return Ok(_applicationUserResponseFactory.Create(user));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to GetUserById");
                return InternalServerError(ex, "Failed to GetUserById");
            }
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            try
            {
                var user = await _applicationUserManager.FindByNameAsync(username);
                if (user != null)
                {
                    return Ok(_applicationUserResponseFactory.Create(user));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to GetUserByName");
                return InternalServerError(ex, "Failed to GetUserByName");
            }
        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserRequestModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationUser appUser = _applicationUserFactory.Create(createUserModel.Username, createUserModel.Email);
                IdentityResult addUserResult = await _applicationUserManager.CreateAsync(appUser, createUserModel.Password);
                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }

                string code = await _applicationUserManager.GenerateEmailConfirmationTokenAsync(appUser.Id);
                var callbackUrl = new Uri(Url.Link(UriName.Identity.Accounts.CONFIRM_EMAIL, new { userId = appUser.Id, code = code }));
                await _applicationUserManager.SendEmailAsync(appUser.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return CreatedAtRoute(UriName.Identity.Accounts.GET_USER, new { id = appUser.Id }, _applicationUserResponseFactory.Create(appUser));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex, "Failed to CreateUser");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = UriName.Identity.Accounts.CONFIRM_EMAIL)]
        public async Task<IHttpActionResult> ConfirmEmail(Guid userId, string code = "")
        {
            if (Guid.Empty == userId || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            try
            {
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
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to ConfirmEmail");
                return InternalServerError(ex, "Failed to ConfirmEmail");
            }
        }

        [Authorize]
        [Route("user/{userID:guid}/changepassword")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangePassword(Guid userID, ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                IdentityResult result = await _applicationUserManager.ChangePasswordAsync(userID, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok("Password changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to ChangePassword");
                return InternalServerError(ex, "Failed to ChangePassword");
            }
        }

        [AllowAnonymous]
        [Route("forgotpassword")]
        [HttpPut]
        public async Task<IHttpActionResult> ForgotPassword(string email = "")
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Valid email field is required.");
            }

            try
            {
                var user = await _applicationUserManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return base.NotFound();
                }
                if (!await _applicationUserManager.IsEmailConfirmedAsync(user.Id))
                {
                    return base.BadRequest("Email is not confirm yet.");
                }

                var code = await _applicationUserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = new Uri(Url.Link(UriName.Identity.Accounts.RESET_PASSWORD, new { userId = user.Id, code = code }));
                await _applicationUserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return Ok("An email send");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to ForgotPassword");
                return InternalServerError(ex, "Failed to ForgotPassword");
            }
        }

        [AllowAnonymous]
        [Route("resetpassword", Name = UriName.Identity.Accounts.RESET_PASSWORD)]
        [HttpPut]
        public async Task<IHttpActionResult> ResetPassword([FromBody] ResetPasswordRequestModel model, [FromUri] Guid userId, [FromUri] string code = "")
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(code))
            {
                return BadRequest("User Id and Code are required");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                IdentityResult result = await _applicationUserManager.ResetPasswordAsync(userId, code, model.NewPassword);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok("Password changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to ResetPassword");
                return InternalServerError(ex, "Failed to ResetPassword");
            }
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(Guid id)
        {

            try
            {
                var appUser = await _applicationUserManager.FindByIdAsync(id);
                if (appUser == null)
                {
                    return NotFound();
                }
                IdentityResult result = await _applicationUserManager.DeleteAsync(appUser);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to DeleteUser");
                return InternalServerError(ex, "Failed to DeleteUser");
            }
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] Guid id, [FromBody] string[] rolesToAssign)
        {

            try
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
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to AssignRolesToUser");
                return InternalServerError(ex, "Failed to AssignRolesToUser");
            }
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] Guid id, [FromBody] List<ClaimRequestModel> claimsToAssign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
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
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to AssignClaimsToUser");
                return InternalServerError(ex, "Failed to AssignClaimsToUser");
            }
        }

        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] Guid id, [FromBody] List<ClaimRequestModel> claimsToRemove)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
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
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to RemoveClaimsFromUser");
                return InternalServerError(ex, "Failed to RemoveClaimsFromUser");
            }
        }

    }
}
