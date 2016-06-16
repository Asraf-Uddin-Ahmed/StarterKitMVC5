using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using $safeprojectname$.Models;
using $safeprojectname$.Models.Account;
using $safeprojectname$.Codes;
using Website.Foundation.Core.Enums;
using Ratul.Mvc;
using Ninject.Extensions.Logging;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Services;
using Ratul.Mvc.Authorization;
using $safeprojectname$.Codes.Core.Services;

namespace $safeprojectname$.Controllers
{
    public class AccountController : BaseController
    {
        private ILogger _logger;
        private IMembershipService _membershipService;
        private IUserService _userService;
        private IValidationMessageService _validationMessageService;
        private IPasswordVerificationService _passwordVerificationService;
        public AccountController(ILogger logger,
            IMembershipService membershipService,
            IUserService userService,
            IPasswordVerificationService passwordVerificationService,
            IValidationMessageService validationMessageService)
            : base(logger)
        {
            _logger = logger;
            _membershipService = membershipService;
            _userService = userService;
            _passwordVerificationService = passwordVerificationService;
            _validationMessageService = validationMessageService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                _validationMessageService.StoreActionResponseMessageError(ModelState);
                return View(model);
            }
            try
            {
                LoginStatus loginStatus = _membershipService.ProcessLogin(model.UserName, model.Password);
                model.StoreActionResponseMessageByLoginStatus(loginStatus);
                if (loginStatus == LoginStatus.Successful)
                {
                    this.StoreUserInSession(_userService.GetUserByUserName(model.UserName));
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                _validationMessageService.StoreActionResponseMessageError("Problem has been occurred while proccessing you requst. Please try again.");
                _logger.Error(ex, "User failed to create: UserName={0}", model.UserName);
            }
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _validationMessageService.StoreActionResponseMessageError(ModelState);
                return View(model);
            }
            try
            {
                User user = model.CreateUser();
                model.SendCofirmEmailIfRequired(user);
                _validationMessageService.StoreActionResponseMessageSuccess("Successfully Registered. Please check your email.");
                return RedirectToAction("Login");
            }
            catch (ArgumentException)
            {
                _validationMessageService.StoreActionResponseMessageError("Invalid Email");
            }
            catch (Exception ex)
            {
                _validationMessageService.StoreActionResponseMessageError("Problem has been occurred while proccessing you requst. Please try again.");
                _logger.Error(ex, "User failed to create: UserName={0}, Email={1}", model.Email, model.Email);
            }
            return View(model);
        }

        //
        // GET: /Account/ConfirmUser
        [AllowAnonymous]
        public ActionResult ConfirmUser(string code)
        {
            try
            {
                VerificationStatus status = _membershipService.VerifyForUserStatus(code);
                if (status == VerificationStatus.Success)
                    return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "User failed to verify: VerificationCode={0}", code);
            }
            _validationMessageService.StoreActionResponseMessageError("Verification Failed");
            return RedirectToAction("Register");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _validationMessageService.StoreActionResponseMessageError(ModelState);
                return View(model);
            }
            try
            {
                if (_userService.IsEmailAddressAlreadyInUse(model.Email))
                {
                    model.ProcessForgotPassword();
                    return RedirectToAction("ForgotPasswordConfirmation");    
                }
                _validationMessageService.StoreActionResponseMessageError("Email does not exist.");
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "User failed to forgot password: VerificationCode={0}", model.Email);
                _validationMessageService.StoreActionResponseMessageError("Your request has been failed while processing. Please try again.");
            }
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ChangePassword
        [AllowAnonymous]
        public ActionResult ChangePassword(string code)
        {
            try
            {
                VerificationStatus status = _membershipService.VerifyForPasswordChange(code);
                if (status == VerificationStatus.Success)
                {
                    User user = _userService.GetUserByPasswordVerificationCode(code);
                    this.StoreUserInSession(user);
                    _passwordVerificationService.RemoveByUserID(user.ID);
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Change password failed to verify: VerificationCode={0}", code);
            }
            _validationMessageService.StoreActionResponseMessageError("Verification Failed");
            return RedirectToAction("Login");
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _validationMessageService.StoreActionResponseMessageError(ModelState);
                return View(model);
            }
            try
            {
                _membershipService.ChangeUserPassword(UserSession.CurrentUser.ID, model.Password);
            }
            catch(Exception ex)
            {
                _validationMessageService.StoreActionResponseMessageError("Password changing failed. Please try again.");
                _logger.Error(ex, "Password changing failed.");
            }
            return RedirectToAction("ChangePasswordConfirmation");
        }

        //
        // GET: /Account/ChangePasswordConfirmation
        [AllowAnonymous]
        public ActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ShowUser
        [OwnerAuthorize]
        public ActionResult ShowUser()
        {
            ShowUserModel model = new ShowUserModel();
            return View(model);
        }

        //
        // GET: /Account/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            UserSession.Clear();
            return RedirectToAction("Login");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private void StoreUserInSession(User user)
        {
            UserSession.CurrentUser = new UserIdentity(user.ID, user.TypeOfUser.ToString(), user.Name);
        }
        #endregion
    }
}