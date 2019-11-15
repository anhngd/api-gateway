using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class PutAccountUnLockCommand : IPutAccountUnLockCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PutAccountUnLockCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Root))
            {
                return new BadRequestObjectResult("ERR_LOCK_ROOT: Unlock root does not allowed.");
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return new BadRequestObjectResult("ERR_LOCK_ADMIN: Unlock administrator does not allowed.");
            }

            // unlock user

            user.LockoutEnabled = false;
            user.LockoutEnd = null;

            await _userManager.UpdateAsync(user);

            return new OkObjectResult("Unlock account success");
        }
    }
}
