using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class PutUnLockUserCommand : IPutUnLockUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PutUnLockUserCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult("User does not exist.");
            }
            // prevent lock root account
            if (await _userManager.IsInRoleAsync(user, RoleNames.Root))
            {
                return new BadRequestObjectResult("ERR_LOCK_ROOT: Unlock root does not allowed.");
            }
            // unlock user
            user.LockoutEnabled = false;
            user.LockoutEnd = null;

            await _userManager.UpdateAsync(user);
            
            return new OkObjectResult("Unlock account success!");
        }
    }
}
