using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class PutAccountLockCommand : IPutAccountLockCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PutAccountLockCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, LockRequestAccount model, CancellationToken cancellationToken)
        {
            // check null argument
            if (model.End == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Root))
            {
                return new BadRequestObjectResult("ERR_LOCK_ROOT: Lock root does not allowed.");
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return new BadRequestObjectResult("ERR_LOCK_ADMIN: Lock administrator does not allowed.");
            }

            // lock user

            user.LockoutEnabled = true;

            if (model?.End == null)
            {
                user.LockoutEnd = null;
            }
            else
            {
                var dtUtc = model.End.Value.ToUniversalTime();
                user.LockoutEnd = dtUtc;
            }

            await _userManager.UpdateAsync(user);
            
            return new OkObjectResult("Lock account success!");
        }
    }
}
