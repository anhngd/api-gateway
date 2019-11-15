using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Commands.AdminCommands
{
    public class DeleteAdminCommand : IDeleteAdminCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteAdminCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(RoleNames.Admin);
            var user = await _userManager.Users
                .Where(u => u.Id == id && u.Roles.Any(r => r.RoleId == role.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return new NotFoundObjectResult("User does not exist.");
            }

            // prevent delete root account
            if (await _userManager.IsInRoleAsync(user, RoleNames.Root))
            {
                return new BadRequestObjectResult("ERR_DEL_ROOT: Delete root does not allowed.");
            }

            await _userManager.DeleteAsync(user);
            
            return new OkObjectResult(null);
        }
    }
}
