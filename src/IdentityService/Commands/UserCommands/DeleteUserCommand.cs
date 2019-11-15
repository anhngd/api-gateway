using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class DeleteUserCommand : IDeleteUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public DeleteUserCommand(UserManager<ApplicationUser> userManager)
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
            if (await _userManager.IsInRoleAsync(user, RoleNames.Root))
            {
                return new BadRequestObjectResult("ERR_DEL_ROOT: Delete root does not allowed.");
            }
            await _userManager.DeleteAsync(user);
            return new OkObjectResult(null);
        }
    }
}
