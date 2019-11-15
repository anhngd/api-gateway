using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.Configurations;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityService.Commands.UserCommands
{
    public class PostUserCommand : IPostUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountConfiguration _accountConfig;
        //private readonly IEmailSender _emailSender;
        private readonly ApiAuthenticationConfiguration _authConfig;
        private readonly IMapper<CreateUser, ApplicationUser> _createUserToApplicationUserMapper;
        private readonly IMapper<ApplicationUser, User> _applicationUserToUserMapper;

        public PostUserCommand(
            UserManager<ApplicationUser> userManager,
            IOptions<AccountConfiguration> accountConfigOptions,
            //IEmailSender emailSender,
            IOptions<ApiAuthenticationConfiguration> authConfigOptions,
            IMapper<CreateUser, ApplicationUser> createUserToApplicationUserMapper,
            IMapper<ApplicationUser, User> applicationUserToUserMapper)
        {
            _userManager = userManager;
            _accountConfig = accountConfigOptions.Value ?? new AccountConfiguration();
            //_emailSender = emailSender;
            _authConfig = authConfigOptions.Value ?? new ApiAuthenticationConfiguration();
            _createUserToApplicationUserMapper = createUserToApplicationUserMapper;
            _applicationUserToUserMapper = applicationUserToUserMapper;
        }

        public async Task<IActionResult> ExecuteAsync(CreateUser model, CancellationToken cancellationToken)
        {
            if (model.Address == null || model.BirthDate == null || model.Email == null || model.FamilyName == null ||
                model.Gender == null || model.GivenName == null || model.Locale == null || model.Password == null ||
                model.PhoneNumber == null || model.Picture == null || model.UserName == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");
            // check user name
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new BadRequestObjectResult("ERR_CFL_USNAME: This user name is already taken. Choose a different name.");
            }
            // check email
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new BadRequestObjectResult("ERR_CFL_EMAIL: This email address is not available. Choose a different address.");
            }
            // Creating new user
            var user = _createUserToApplicationUserMapper.Map(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return new BadRequestObjectResult(errors);
            }
            // Check if required confirm email
            if (_accountConfig.RequireConfirmEmail)
            {
                // Generate confirm url and send to user's email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var link = Url.Link(
                //     "ConfirmEmailRoute", new {uid = user.Id, token = code});
                var encodedUid = HttpUtility.UrlEncode(user.Id);
                var encodedToken = HttpUtility.UrlEncode(code);

                var link = (_authConfig.ConfirmEmailUrlTemplate ?? "")
                            .Replace("{uid}", encodedUid)
                            .Replace("{token}", encodedToken);

                //await _emailSender.SendConfirmNewAccountAsync(
                //    user.Email, "Confirm your new account", link, user.GivenName);
            }

            //var itemUrl = Url.Action(nameof(GetById), "Accounts", new
            //{
            //    id = user.Id
            //});

            var url = "/api/users/" + user.Id;
            var viewmodel = _applicationUserToUserMapper.Map(user);
            return new CreatedResult(url, viewmodel);
        }
    }
}
