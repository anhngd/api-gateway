using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AdminCommands
{
    public class DeleteAdminUserCommand : IDeleteAdminUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteAdminUserCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult($"User with ID '{id}' does not exist.");
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.Admin);
            }

            return new OkObjectResult(null);
        }
    }
}
