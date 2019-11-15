using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Commands.AdminCommands
{
    public class GetAdminCommand : IGetAdminCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper<ApplicationUser, Admin> _adminMapper;

        public GetAdminCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper<ApplicationUser, Admin> adminMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _adminMapper = adminMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(RoleNames.Admin);
            var user = await _userManager.Users
                .Where(u => u.Id == id && u.Roles.Any(r => r.RoleId == role.Id))
                .FirstOrDefaultAsync();
            if (user == null)
                return new NotFoundObjectResult("Account with id was not found!");
            var viewmodel = _adminMapper.Map(user);
            return new OkObjectResult(viewmodel);
        }
    }
}
