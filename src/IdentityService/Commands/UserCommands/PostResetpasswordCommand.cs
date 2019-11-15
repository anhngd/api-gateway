using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityService.Library;
using IdentityService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class PostResetPasswordCommand : IPostResetPasswordCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _context;

        public PostResetPasswordCommand(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(ResetPassword model, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return new NotFoundObjectResult("Not found account with id " + model.UserId);

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors;
                var message = new List<string>();
                foreach(var error in errors)
                {
                    message.Add(error.Description);
                }
                return new BadRequestObjectResult(message);
                //return new BadRequestObjectResult("Reset password failed! Please try again in a few minutes!");
            }

            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "reset");

            return new OkObjectResult("Password is updated!");
        }
    }
}
