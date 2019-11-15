using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AdminCommands
{
    public class PutAdminPermissionCommand : IPutAdminPermissionCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PutAdminPermissionCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string note, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByIdAsync("");
            return new OkResult();
        }
    }
}
