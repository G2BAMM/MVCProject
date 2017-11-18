/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : AccountController.cs              '
'  Description      : Manages custom account logic      ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 17 Oct 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVCWebProject2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using MVCWebProject2.utilities;
using System.Web.Configuration;
using System.Net;
using System.Security.Claims;

namespace MVCWebProject2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        
        [AllowAnonymous]
        public ActionResult ConfirmUpdate()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmRegistration()
        {
            return View();
        }

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
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var loginManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var loginUser = loginManager.FindByEmail(model.Email);
            if (loginUser != null)
            {
                if (loginUser.EmailConfirmed == false)
                {
                    //This user is attempting to sign in before confirming their email address so advise accordingly
                    ViewBag.AttemptSignIn = true;
                    return View("ConfirmRegistration");
                }
            }
           
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //There is no http context yet at this stage of the process so we need to get the details from the ResponseGrant instead
                    var Grant = SignInManager.AuthenticationManager.AuthenticationResponseGrant;
                    //Get our authenticated user from the response grant
                    var currentUserId = Grant.Identity.GetUserId();
                    //Set up our user manager
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    //Get the details (whole row from SQL table) for our local user from the SQL user manager
                    var currentUser = manager.FindById(currentUserId);
                    //Create our login cookie to be able to use the custom fields from AspNetUser table
                    var cookies = new CookieManager();
                    cookies.WriteCookie(currentUser);
                    if(manager.IsInRole(currentUser.Id, "Super Admin") || manager.IsInRole(currentUser.Id, "Admin"))
                    {
                        //This is the admin so redirect to the admin home page
                        return Redirect("~/Admin/Home");
                    }
                    else
                    {
                        //Not admin users so redirect to the default home page
                        return RedirectToLocal(returnUrl);
                    }
                    
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
    

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //Owin sign out
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //Delete our custom cookie
            var cookies = new CookieManager();
            //Set the default theme 
            cookies.ClearCookie();
            //Returning to the home page
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
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
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, BootstrapTheme = model.BootstrapTheme, FirstName = model.FirstName, Surname = model.Surname };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //User was successfully created so now add the to the users role
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    //Does our role exist?
                    if (!roleManager.RoleExists("User"))
                    {
                        // Role doesn't exist so create it now    
                        var role = new IdentityRole
                        {
                            Name = "User"
                        };
                        roleManager.Create(role);
                    }
                    //Add the user to the 'User' role                   
                    UserManager.AddToRole(user.Id, "User");

                    //Finally sign the user in now
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    if (!roleManager.RoleExists("Admin"))
                    {
                        // first we create Admin rool    
                        var adminRole = new IdentityRole();
                        adminRole.Name = "Admin";
                        roleManager.Create(adminRole);

                        //Create a Admin who will maintain the website content                   

                        var adminUser = new ApplicationUser();
                        adminUser.UserName = "admin@easyhire.com";
                        adminUser.Email = "admin@easyhire.com";
                        adminUser.BootstrapTheme = "Standard";
                        adminUser.FirstName = "Admin";
                        adminUser.Surname = "Account";
                        adminUser.EmailConfirmed = true;

                        string adminPWD = "Password1!";

                        var chkUser = UserManager.Create(adminUser, adminPWD);

                        //Add default User to Role Admin    
                        if (chkUser.Succeeded)
                        {
                            var result1 = UserManager.AddToRole(adminUser.Id, "Admin");

                        }

                        if (!roleManager.RoleExists("Super Admin"))
                        {
                            //Create a Super Admin role    
                            var superAdminRole = new IdentityRole();
                            superAdminRole.Name = "Super Admin";
                            roleManager.Create(superAdminRole);

                            //Create a Super Admin super user who will maintain the website security                   

                            var superAdminuser = new ApplicationUser();
                            superAdminuser.UserName = WebConfigurationManager.AppSettings["MailAdminAccount"];
                            superAdminuser.Email = WebConfigurationManager.AppSettings["MailAdminAccount"];
                            superAdminuser.BootstrapTheme = "Standard";
                            superAdminuser.FirstName = "Super Admin";
                            superAdminuser.Surname = "Account";
                            superAdminuser.EmailConfirmed = true;

                            string superAdminPWD = WebConfigurationManager.AppSettings["MailAdminPassword"]; ;

                            var chkSuperUser = UserManager.Create(superAdminuser, superAdminPWD);

                            //Add default User to Role Admin    
                            if (chkUser.Succeeded)
                            {
                                var result2 = UserManager.AddToRole(superAdminuser.Id, "Super Admin");

                            }
                        }
                    }

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //Generate a custom message
                    var mailMessage = $"<html><body>" +
                                      $"<p>Dear {model.FirstName},</p>" +
                                      $"<p>Thank you for signing up with us today, we just need you to confirm your identity.</p>" +
                                      $"<p>Please confirm your account by clicking <a href=\"{callbackUrl}\">here</a></p>" +
                                      $"<p>Thank you,</p>" +
                                      $"<p>Easy Hire Admin</p>" +
                                      $"</body></html>";
                    //Now send the mail using identity config set up for OWIN
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", mailMessage);
                    ViewBag.AttemptSignIn = false;
                    return View("ConfirmRegistration");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
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
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //Generate a custom message
                var mailMessage = $"<html><body>" +
                                  $"<p>Dear User,</p>" +
                                  $"<p>Please reset your password by clicking <a href=\"{callbackUrl}\">here</a></p>" +
                                  $"<p>Thank you,</p>" +
                                  $"<p>Easy Hire Admin</p>" +
                                  $"</body></html>";
                await UserManager.SendEmailAsync(user.Id, "Reset Password", mailMessage);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
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
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();

            
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            
            var result = SignInStatus.Failure;
            if (loginInfo == null)
            {
                //User was not found on 3rd party login system
                return RedirectToAction("ExternalLoginFailure");
            }
            else
            {
                //User was found on the 3rd party system so process the custom login now
                var loginManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                //Check to see if a local user account exists with this email address
                if (loginInfo.Email == null || loginInfo.Email == "")
                {
                    return RedirectToAction("ExternalLoginFailure");
                }
                var loginUser = await loginManager.FindByEmailAsync(loginInfo.Email);
                //Local user account was found
                if (loginUser != null)
                {
                    //Local user account was found so we can log them in now
                    result = SignInStatus.Success;
                    //Set a new db context to update the current users record
                    var updateUser = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    //Reload the user details from SQL
                    var confirmedUser = updateUser.FindById(loginUser.Id);
                    if (!confirmedUser.EmailConfirmed)
                    { 
                        //Update the verified email flag as we can now confirm this exists from the 3rd party login success
                        confirmedUser.EmailConfirmed = true;
                        //Update the user account back to SQL
                        await updateUser.UpdateAsync(confirmedUser);
                    }
                    //Now logon our user using the original usermanager record
                    SignInManager.SignIn(loginUser, false, false);
                    //There is no http context yet at this stage of the process so we need to get the details from the ResponseGrant instead
                    var Grant = SignInManager.AuthenticationManager.AuthenticationResponseGrant;
                    //Get our authenitcated user from the response grant
                    var currentUserId = Grant.Identity.GetUserId();
                    //Set up our user manager
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    //Get the details (whole row from SQL table) for our local user from the SQL user manager
                    var currentUser = manager.FindById(currentUserId);
                    //Create our login cookie to be able to use the custom fields from AspNetUser table
                    var cookies = new CookieManager();
                    cookies.WriteCookie(currentUser);
                }

            }
            
            // Sign in the user with this external login provider if the user already has a login
            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            
            
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
           
            
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, BootstrapTheme = model.BootstrapTheme, FirstName = model.FirstName, Surname = model.Surname, EmailConfirmed = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //User was successfully created so now add the to the users role
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    //Does our role exist?
                    if (!roleManager.RoleExists("User"))
                    {
                        // Role doesn't exist so create it now    
                        var role = new IdentityRole
                        {
                            Name = "User"
                        };
                        roleManager.Create(role);
                    }
                    //Add the user to the 'User' role                   
                    UserManager.AddToRole(user.Id, "User");

                    //Finally sign the user in now
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    if (!roleManager.RoleExists("Admin"))
                    {
                        //Create an Admin role   
                        var adminRole = new IdentityRole();
                        adminRole.Name = "Admin";
                        roleManager.Create(adminRole);

                        //Create an Admin who will maintain the website content                   

                        var adminUser = new ApplicationUser();
                        adminUser.UserName = "admin@easyhire.com";
                        adminUser.Email = "admin@easyhire.com";
                        adminUser.BootstrapTheme = "Standard";
                        adminUser.FirstName = "Admin";
                        adminUser.Surname = "Account";
                        adminUser.EmailConfirmed = true;

                        string adminPWD = "Password1!";

                        var chkUser = UserManager.Create(adminUser, adminPWD);

                        //Add default User to Role Admin    
                        if (chkUser.Succeeded)
                        {
                            var result1 = UserManager.AddToRole(adminUser.Id, "Admin");

                        }

                        if (!roleManager.RoleExists("Super Admin"))
                        {
                            //Create a Super Admin role    
                            var superAdminRole = new IdentityRole();
                            superAdminRole.Name = "Super Admin";
                            roleManager.Create(superAdminRole);

                            //Create a Super Admin super user who will maintain the website security                   

                            var superAdminUser = new ApplicationUser();
                            superAdminUser.UserName = WebConfigurationManager.AppSettings["MailAdminAccount"];
                            superAdminUser.Email = WebConfigurationManager.AppSettings["MailAdminAccount"];
                            superAdminUser.BootstrapTheme = "Standard";
                            superAdminUser.FirstName = "Super Admin";
                            superAdminUser.Surname = "Account";
                            superAdminUser.EmailConfirmed = true;

                            string superAdminPWD = WebConfigurationManager.AppSettings["MailAdminPassword"]; ;

                            var chkSuperUser = UserManager.Create(superAdminUser, superAdminPWD);

                            //Add default User to Role Admin    
                            if (chkUser.Succeeded)
                            {
                                var result2 = UserManager.AddToRole(superAdminUser.Id, "Super Admin");

                            }
                        }
                    }
                    //Sign the user in now
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //There is no http context yet at this stage of the process so we need to get the details from the ResponseGrant instead
                    var Grant = SignInManager.AuthenticationManager.AuthenticationResponseGrant;
                    //Get our authenitcated user from the response grant
                    var currentUserId = Grant.Identity.GetUserId();
                    //Set up our user manager
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    //Get the details (whole row from SQL table) for our local user from the SQL user manager
                    var currentUser = manager.FindById(currentUserId);
                    //Create our login cookie to be able to use the custom fields from AspNetUser table
                    var cookies = new CookieManager();
                    cookies.WriteCookie(currentUser);
                    return Redirect("~/Manage/ConfirmExternalRegister");
                }
               AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);  
        }

        

        

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}