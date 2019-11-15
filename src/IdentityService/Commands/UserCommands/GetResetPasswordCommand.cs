using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class GetResetPasswordCommand : IGetResetPasswordCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetResetPasswordCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string userId, string token, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new NotFoundObjectResult("Not found account with id " + userId);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult("Email verification failed. Please try again in a few minutes!");
            }
            
            return new OkObjectResult(
                new
                {
                    userId = user.Id,
                    token = await _userManager.GeneratePasswordResetTokenAsync(user),
                }
            );
        }
    }
}
