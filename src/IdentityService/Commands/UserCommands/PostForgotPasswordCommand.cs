using System.Threading;
using System.Threading.Tasks;
using System.Web;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityService.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class PostForgotPasswordCommand : IPostForgotPasswordCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _context;

        public PostForgotPasswordCommand(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return new NotFoundObjectResult("Not found account with email " + email);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenEncode = HttpUtility.UrlEncode(token);
            EmailExtension.SendEmail("FORGOT_PASSWORD", user.Email, $"{user.FamilyName} {user.GivenName}", user.Id, tokenEncode);
            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "forgot");
            return new OkObjectResult("A confirm email was sent!");
        }
    }
}
