using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class PutAccountUserCommand : IPutAccountUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PutAccountUserCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, string username, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new BadRequestObjectResult($"User with ID '{id}' does not exist.");
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.User))
            {
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }
            
            return new OkObjectResult("Link a system user to a customer account success!");
        }
    }
}
