/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : ManageController.cs               '
'  Description      : Manages account management logic  ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 23 Oct 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVCWebProject2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace MVCWebProject2.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public ActionResult Index()
        {
            /*
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            */
            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(userId);
            var model = new UpdateDetailsViewModel
            {
                PhoneNumber = currentUser.PhoneNumber,
                FirstName = currentUser.FirstName,
                Surname = currentUser.Surname,
                Email = currentUser.Email,
                BootstrapTheme = currentUser.BootstrapTheme,
                NewPassword = currentUser.PasswordHash,
                ConfirmPassword = currentUser.PasswordHash,
                OldPassword = currentUser.PasswordHash

            };

            ViewBag.Name = User.Identity.Name;

            return View(model);
        }

        //
        // POST: /Manage/Update Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(UpdateDetailsViewModel model)
        {
            //Set these flags so we know what was changed
            var emailVerified = true;
            var passwordChanged = false;
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            /*
             * Now we need to make the changes saved on the form and take appropriate action[s] if email address or passwords were updated
             * as we need to log the user out in these circumstances but we need to save any theme, name changes or tele number changes
             * before we update any email address or passwords.
             * 
             */
            if (model.OldPassword == null && model.NewPassword != null)
            {
                ModelState.AddModelError("OldPassword", "Please enter your old password first when making changes to it!");
                return View(model);
            }

            //Now we can retrieve the user details from SQL as we have no form errors and we will need to compare everything to decide what we do with the current login session
            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(userId);

            if (ChangedPassword(model))
            {
                //We're updating our password so we can attempt to do that now 
                //we will perform the required logout after the rest of any changes are applied
                var pwdResult = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (pwdResult.Succeeded)
                {
                    passwordChanged = true;
                }
                else
                {
                    AddErrors(pwdResult);
                    return View(model);
                }
                
            }

            if (ChangedEmailAddress(model))
            {
                //Update the user's email address to the one entered on the form
                var emailResult = await UserManager.SetEmailAsync(User.Identity.GetUserId(), model.Email);
                //Did the change of email address happen?
                if (emailResult.Succeeded)
                {
                    //Email update was successful
                    emailVerified = false;
                    //Generate a new security token
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(currentUser.Id);
                    //Generate the dynamic link for the email to be sent to the user to confirm the new address
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = currentUser.Id, code = code }, protocol: Request.Url.Scheme);
                    //Generate a custom email message
                    var mailMessage = $"<html><body>" +
                                      $"<p>Dear {model.FirstName},</p>" +
                                      $"<p>Thank you for updating your account with us today, we just need you to confirm your new email address now.</p>" +
                                      $"<p>Please confirm your account by clicking <a href=\"{callbackUrl}\">here</a></p>" +
                                      $"<p>Thank you,</p>" +
                                      $"<p>Easy Hire Admin</p>" +
                                      $"</body></html>";
                    //Send email verification to the user 
                    await UserManager.SendEmailAsync(currentUser.Id, "Confirm your email", mailMessage);
                }
                else
                //Email update was not successful so show the error message[s] and return the completed form to the user
                { 
                    AddErrors(emailResult);
                    return View(model);
                }
            }

            //Update our user details to reflect any changes made to other areas on the form
            currentUser.BootstrapTheme = model.BootstrapTheme;
            currentUser.FirstName = model.FirstName;
            currentUser.Surname = model.Surname;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.UserName = model.Email;
            //Perform general account update
            var result = await UserManager.UpdateAsync(currentUser);
            //Was the update successful?
            if (result.Succeeded)
            {
            //General update was successful
                if (!emailVerified || passwordChanged)
                //Email or password was changed
                {
                    //Sign user out
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //Delete our custom cookie
                    Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(-1);
                    //Email was changed
                    if (!emailVerified)
                    {
                        //Send the user to the email verification page
                        ViewBag.AttemptSignIn = false;
                        return Redirect("~/Account/ConfirmRegistration");
                        
                    }else
                    //Password was changed
                    {
                        //Send the user to the login page now
                        return Redirect("~/Account/Login");
                    }
                }
                else
                //Email or password was not changed 
                {
                    //Update the user login details and cookie
                    Response.Cookies["userInfo"]["BootstrapTheme"] = currentUser.BootstrapTheme;
                    Response.Cookies["userInfo"]["FirstName"] = currentUser.FirstName;
                    Response.Cookies["userInfo"]["Surname"] = currentUser.Surname;
                    //Redirect to confirm update view as the cookie for the theme (if it was changed) will only apply on next http request
                    return Redirect("~/Account/ConfirmUpdate");
                }
                
            }
            //General update was not successful so show the error message[s] and return the completed form to the user.
            AddErrors(result);
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        private bool ChangedPassword(UpdateDetailsViewModel model)
        {
            if (model.NewPassword != null)
            {
                return true;
            }
            return false;
        }

        private bool ChangedEmailAddress(UpdateDetailsViewModel model)
        {
            if (model.Email != User.Identity.GetUserName())
            {
                return true;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}