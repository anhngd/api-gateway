using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityService.Configurations;
using IdentityService.Library;
using IdentityService.ViewModels.SignUpViewModels;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityService.Commands.UserCommands
{
    public class PostSignUpCommand : IPostSignUpCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountConfiguration _accountConfig;
        private readonly ApiAuthenticationConfiguration _authConfig;
        private readonly IMapper<ApplicationUser, User> _applicationUserToUserMapper;
        private readonly IMapper<SignUp, ApplicationUser> _signUpToApplicationUserMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _context;

        public PostSignUpCommand(
            UserManager<ApplicationUser> userManager,
            IOptions<AccountConfiguration> accountConfigOptions,
            IOptions<ApiAuthenticationConfiguration> authConfigOptions,
            IMapper<ApplicationUser, User> applicationUserToUserMapper,
            IMapper<SignUp, ApplicationUser> signUpToApplicationUserMapper,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _accountConfig = accountConfigOptions.Value ?? new AccountConfiguration();
            _authConfig = authConfigOptions.Value ?? new ApiAuthenticationConfiguration();
            _applicationUserToUserMapper = applicationUserToUserMapper;
            _signUpToApplicationUserMapper = signUpToApplicationUserMapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(SignUp model, CancellationToken cancellationToken)
        {
            // check required fields
            if (model.GivenName == null || model.FamilyName == null || model.Email == null || model.Password == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");

            // check email
            if (await _userManager.FindByNameAsync(model.Email) != null)
            {
                return new ConflictObjectResult("ERR_CFL_EMAIL: This email address is not available. Choose a different email address.");
            }
            // check email
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new ConflictObjectResult("ERR_CFL_EMAIL: This email address is not available. Choose a different email address.");
            }
            // create new account
            var user = _signUpToApplicationUserMapper.Map(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result);
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
                    // ignored
                }

                return new BadRequestObjectResult(result);
            }
            
            // Send confirm email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            EmailExtension.SendEmail("REGISTER", user.Email, $"{user.FamilyName} {user.GivenName}", user.Id, HttpUtility.UrlEncode(token));
            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "register");
            return new OkObjectResult("Sign up successfully with email "+user.Email+"! You need confirm your email!");
        }
    }
}