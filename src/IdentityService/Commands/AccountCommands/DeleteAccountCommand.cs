using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class DeleteAccountCommand : IDeleteAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteAccountCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);

            // check if user exists
            if (user == null)
            {
                return new NotFoundObjectResult("User does not exist.");
            }

            // prevent delete root
            if (
                await _userManager.IsInRoleAsync(user, RoleNames.Root) ||
                await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return new BadRequestObjectResult("ERR_DEL_ROOT: Delete root or administrator account does not allowed.");
            }

            // perform delete
            await _userManager.DeleteAsync(user);

            return new OkObjectResult(null);
        }
    }
}
