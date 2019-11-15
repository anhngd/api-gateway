using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class PostConfirmEmailCommand : IPostConfirmEmailCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostConfirmEmailCommand(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string userId, string token, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new NotFoundResult();

            if (user.EmailConfirmed)
                return new OkObjectResult("Email was confirmed!");

            //var token2 = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return new BadRequestObjectResult("Email confirmation failed!");

            // Update Email confirm status
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            
            return new OkObjectResult("Confirm email successfully!");
        }
    }
}
