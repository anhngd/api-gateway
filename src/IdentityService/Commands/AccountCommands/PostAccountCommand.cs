using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.Configurations;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityService.Commands.AccountCommands
{
    public class PostAccountCommand : IPostAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountConfiguration _accountConfig;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IEmailSender _emailSender;
        private readonly ApiAuthenticationConfiguration _authConfig;
        private readonly IMapper<CreateAccount, ApplicationUser> _createAccountMapper;
        private readonly IMapper<ApplicationUser, Account> _accountMapper;

        public PostAccountCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AccountConfiguration> accountConfigOptions,
            //IEmailSender emailSender,
            IOptions<ApiAuthenticationConfiguration> authConfigOptions,
            IMapper<CreateAccount, ApplicationUser> createAccountMapper,
            IMapper<ApplicationUser, Account> accountMapper)
        {
            _userManager = userManager;
            _accountConfig = accountConfigOptions.Value ?? new AccountConfiguration();
            _roleManager = roleManager;
            //_emailSender = emailSender;
            _authConfig = authConfigOptions.Value ?? new ApiAuthenticationConfiguration();
            _createAccountMapper = createAccountMapper;
            _accountMapper = accountMapper;
        }

        public async Task<IActionResult> ExecuteAsync(CreateAccount model, CancellationToken cancellationToken)
        {
            // check null argument
            if (model.Address == null || model.BirthDate == null || model.Email == null || model.FamilyName == null ||
                model.Gender == null || model.GivenName == null || model.Locale == null || model.Password == null ||
                model.PhoneNumber == null || model.Picture == null || model.UserName == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");
            // Check user name
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new BadRequestObjectResult("ERR_CFL_USNAME: This user name is already taken. Choose a different name.");
            }
            // check email
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new BadRequestObjectResult("ERR_CFL_EMAIL: This email address is not available. Choose a different address.");
            }
            // create account
            var user = _createAccountMapper.Map(model);
            var role = await _roleManager.FindByNameAsync(RoleNames.User);
            user.RoleId = role.Id;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return new BadRequestObjectResult(errors);
            }
            // add user to role
            result = await _userManager.AddToRoleAsync(user, RoleNames.User);
            if (!result.Succeeded)
            {
                try
                {
                    await _userManager.DeleteAsync(user);
                }
                catch
                {

                }
                var errors = result.Errors
                    .Select(e => new BadRequestObjectResult(
                    new {
                        Code = e.Code,
                        Message = e.Description
                    }))
                    .ToArray();
                return new BadRequestObjectResult(errors);
            }
            // Check if required confirm email
            if (_accountConfig.RequireConfirmEmail)
            {
                // Generate confirm url and send to user's email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var link = Url.Link("ConfirmEmailRoute", new {uid = user.Id, token = code});
                var encodedUid = HttpUtility.UrlEncode(user.Id);
                var encodedToken = HttpUtility.UrlEncode(code);

                var link = (_authConfig.ConfirmEmailUrlTemplate ?? "")
                            .Replace("{uid}", encodedUid)
                            .Replace("{token}", encodedToken);

                var callbackUrl = new Uri(link);

                //await _emailSender.SendConfirmNewAccountAsync(
                //                    user.Email,
                //                    "Confirm your new account",
                //                    callbackUrl,
                //                    user.GivenName);
            }

            //var itemUrl = Url.Action(nameof(GetById), ControllerName, new
            //{
            //    id = user.Id
            //});

            var url = "/api/accounts/" + user.Id;
            var viewmodel = _accountMapper.Map(user);
            return new CreatedResult(url, viewmodel);
        }
    }
}
