using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IdentityServer.Common;
using Microsoft.AspNetCore.Http;
using IdentityServer.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace IdentityServer.Public.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IdentityDbContext _context;
        private readonly IStringLocalizer<AccountController> _localizer;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IdentityDbContext context,
            IStringLocalizer<AccountController> localizer)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _context = context;
            _localizer = localizer;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }

                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        // write activity history
                        await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "login");
                        return Redirect("~/");
                    }
// user might have clicked on a malicious link - should be logged
                    throw new Exception("invalid return URL");
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            ViewData["message"] = _localizer["Email or password wrong!"];
            return View(vm);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User?.Identity.IsAuthenticated == false)
                return Redirect("~/account/login");

            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }
            else
            {
                return Redirect("~/account/login");
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                var url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            //_context.LoginHistories.Where(x=>x.Id == )

            return View("LoggedOut", vm);
            //return Redirect("~/account/login");
        }



        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                    ExternalProviders = new ExternalProvider[] { new ExternalProvider { AuthenticationScheme = context.IdP } }
                };
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            var vm = new ForgotPasswordViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, string button)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ViewData["message"] = _localizer["Email is not avaiable! Please try again!"]; 
                model.IsEmailSent = 1;
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewData["message"] = _localizer["Email is not avaiable! Please try again!"];
                model.IsEmailSent = 1;
            }
            else
            {
                var appsettingValue = ConfigurationExtension.AppSetting;
                // generate token link
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var tokenEncode = HttpUtility.UrlEncode(token);
                var linkConfirm = "/account/resetpassword?userId=" + user.Id + "&token=" + tokenEncode;

                var emailToSend = user.Email;
                var recvName = user.GivenName + " " + user.FamilyName;
                EmailExtension.SendEmail(emailToSend, recvName, linkConfirm, "FORGOT_PASSWORD");

                // write activity history
                await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "forgot");

                ViewData["message"] = string.Format(emailToSend);
                model.IsEmailSent = 2;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            var vm = new ResetPasswordViewModel();
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewData["message"] = string.Format(_localizer["Email verification failed. Please try again in a few minutes!"]);
                vm.ConfirmEmail = 0;
                return View(vm);
            }

            //token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded)
            {
                ViewData["message"] = string.Format(_localizer["Email verification failed. Please try again in a few minutes!"]);
                vm.ConfirmEmail = 0;
                return View(vm);
            }

            vm.UserId = userId;
            vm.Token = await _userManager.GeneratePasswordResetTokenAsync(user);
            vm.NewPassword = "";
            vm.ConfirmEmail = 1;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string button)
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            var vm = new ResetPasswordViewModel();

            if(model.NewPassword == null || model.NewPassword == string.Empty)
            {
                vm.ConfirmEmail = 2;
                ViewData["message"] = string.Format(_localizer["Password is required!"]);
                return View(vm);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null)
            {
                ViewData["message"] = string.Format(_localizer["Reset password failed! Please try again in a few minutes!"]);
                vm.ConfirmEmail = 2;
                return View(vm);
            }
            //model.Token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if(result.Succeeded)
            {
                ViewData["message"] = string.Format(_localizer["Reset password failed! Please try again in a few minutes!"]);
                vm.ConfirmEmail = 2;
                return View(vm);
            }

            // write activity history
            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "reset");

            ViewData["message"] = string.Format(_localizer["Reset password success! You can login with new password!"]);
            vm.ConfirmEmail = 3;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePassword()
        {
            if (User?.Identity.IsAuthenticated == false)
                return Redirect("~/account/login");
            var model = new UpdatePasswordViewModel();
            model.IsChangePass = 0;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model, string button)
        {
            if (User?.Identity.IsAuthenticated == false)
                return Redirect("~/account/login");

            if (model.CurrentPassword == null || model.CurrentPassword == string.Empty)
            {
                model.IsChangePass = 1;
                ViewData["message"] = string.Format(_localizer["Current password is required!"]);
                return View(model);
            }
            if (model.NewPassword == null || model.NewPassword == string.Empty)
            {
                model.IsChangePass = 1;
                ViewData["message"] = string.Format(_localizer["New password is required!"]);
                return View(model);
            }
            if (model.ConfirmNewPassword != model.NewPassword)
            {
                model.IsChangePass = 1;
                ViewData["message"] = string.Format(_localizer["Confirm new password not matches!"]);
                return View(model);
            }


            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (user == null)
                {
                    model.IsChangePass = 1;
                    ViewData["message"] = string.Format(_localizer["Change password failed! Try again!"]);
                    return View(model);
                }

                var claims = user.Claims.ToList();
                if (claims.Count < 1)
                {
                    model.IsChangePass = 1;
                    ViewData["message"] = string.Format(_localizer["Change password failed! Try again!"]);
                    return View(model);
                }

                var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;
                var account = await _userManager.FindByIdAsync(userId);

                var result = await _userManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    model.IsChangePass = 1;
                    ViewData["message"] = string.Format(_localizer["Change password failed! Try again!"]);
                }
                else
                {
                    var emailToSend = account.Email;
                    var recvName = account.GivenName + " " + account.FamilyName;
                    EmailExtension.SendEmail(emailToSend, recvName, "", "UPDATE_PASSWORD");

                    // write activity history
                    await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, account, "update");

                    model.IsChangePass = 2;
                    ViewData["message"] = string.Format(_localizer["Change password success!"]);
                }
            }
            catch
            {
                model.IsChangePass = 1;
                ViewData["message"] = string.Format(_localizer["Change password failed! Try again!"]);
            }

            return View(model);
        }

        [HttpGet]
        //[Authorize("Admin,Root")]
        public async Task<IActionResult> Register()
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            var model = new RegisterViewModel();
            model.Result = false;
            model.Message = "";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize("Admin,Root")]
        public async Task<IActionResult> Register(RegisterViewModel model, string button)
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            var resultValidate = ValidationRegister(model);
            if (resultValidate != "ok")
            {
                model.Result = false;
                model.Message = resultValidate;
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                model.Result = false;
                model.Message = _localizer["You already have a Tripbricks.com account registered to this email: "] + model.Email + _localizer[". You can sign in directly!"];
                return View(model);
            }

            var account = new ApplicationUser();
            account.FamilyName = model.FamilyName;
            account.GivenName = model.GivenName;
            account.Email = model.Email;
            account.UserName = model.Email;

            var result = await _userManager.CreateAsync(account, model.Password);
            if (!result.Succeeded)
            {
                model.Result = false;
                model.Message = _localizer["Can not create account! Try again!"];
                return View(model);
            }
            result = await _userManager.AddToRoleAsync(account, "OnlineUser");
            if (!result.Succeeded)
            {
                try
                {
                    await _userManager.DeleteAsync(account);
                }
                catch { }
                model.Result = false;
                model.Message = _localizer["Can not create account! Try again!"];
                return View(model);
            }

            // send email
            var emailToSend = account.Email;
            var recvName = account.GivenName + " " + account.FamilyName;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);
            var tokenEncode = HttpUtility.UrlEncode(token);
            var linkConfirm = "/account/verifyemail?userId=" + account.Id + "&token=" + tokenEncode;

            EmailExtension.SendEmail(emailToSend, recvName, linkConfirm, "REGISTER");

            // write activity history
            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, account, "register");

            model.Result = true;
            model.Message = _localizer["Your account has been successfully created. An email has been sent to you with detailed instructions on how to activate it!"];
            return View(model);
        }

        private string ValidationRegister(RegisterViewModel model)
        {
            if (model.FamilyName == null || model.FamilyName == string.Empty)
            {
                return _localizer["Family name is required!"];
            }
            if (model.GivenName == null || model.GivenName == string.Empty)
            {
                return _localizer["Given name is required!"];
            }
            if (model.Email == null || model.Email == string.Empty)
            {
                return _localizer["Email is required!"];
            }
            if (model.Password == null || model.Password == string.Empty)
            {
                return _localizer["Password is required!"];
            }

            var validationPassword = ValidationExtension.ValidationPassword(model.Password);
            if (validationPassword != "ok")
            {
                return validationPassword;
            }

            if (model.ConfirmPassword != model.Password)
            {
                return _localizer["Confirm password not matches!"];
            }

            return _localizer["ok"];
        }

        [HttpGet]
        //[Authorize("Admin,Root")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (User?.Identity.IsAuthenticated == true)
                return Redirect("~/");

            var model = new VerifyEmailViewModel();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                model.ConfirmResult = false;
                model.Message = _localizer["Email verification failed. Please try again in a few minutes!"];
                return View(model);
            }

            if (user.EmailConfirmed)
            {
                model.ConfirmResult = true;
                model.Message = _localizer["Email email has been confirmed!"];
                return View(model);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                model.ConfirmResult = false;
                model.Message = _localizer["Email verification failed. Please try again in a few minutes!"];
                return View(model);
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            model.ConfirmResult = true;
            model.Message = _localizer["Verify email success!"];
            return View(model);
        }
    }
}