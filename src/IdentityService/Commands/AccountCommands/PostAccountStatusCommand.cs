using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class PostAccountStatusCommand : IPostAccountStatusCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostAccountStatusCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ExecuteAsync(string id, string note, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByIdAsync("");
            return new OkResult();
        }
    }
}
