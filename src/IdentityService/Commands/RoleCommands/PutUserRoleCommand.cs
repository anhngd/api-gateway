using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Models;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.RoleCommands
{
    public class PutUserRoleCommand : IPutUserRoleCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PutUserRoleCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
            
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> ExecuteAsync(UpdateUserRole model, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new NotFoundObjectResult("User does not exist.");
            }
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return new NotFoundObjectResult("Role does not exist.");
            }
            user.RoleId = role.Id;
            await _userManager.UpdateAsync(user);
            return new OkObjectResult(user);
        }
    }
}
