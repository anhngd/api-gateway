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
    public class PutAdminCommand : IPutAdminCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper<ApplicationUser, Admin> _adminMapper;
        private readonly IMapper<UpdateAdmin, ApplicationUser> _updateAdminMapper;

        public PutAdminCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper<ApplicationUser, Admin> adminMapper,
            IMapper<UpdateAdmin, ApplicationUser> updateAdminMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _adminMapper = adminMapper;
            _updateAdminMapper = updateAdminMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string id, UpdateAdmin model, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(RoleNames.Admin);
            var user = await _userManager.Users
                .Where(u => u.Id == id && u.Roles.Any(r => r.RoleId == role.Id))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user == null)
            {
                return new NotFoundObjectResult("User does not exist.");
            }

            _updateAdminMapper.Map(model, user);
            await _userManager.UpdateAsync(user);

            var viewmodel = _adminMapper.Map(user);
            return new OkObjectResult(viewmodel);
        }
    }
}
