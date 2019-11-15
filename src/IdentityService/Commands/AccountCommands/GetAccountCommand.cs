using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Commands.AccountCommands
{
    public class GetAccountCommand : IGetAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper<ApplicationUser, Account> _accountMapper;

        public GetAccountCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper<ApplicationUser, Account> accountMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accountMapper = accountMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var roleAccount = await _roleManager.FindByNameAsync(RoleNames.User);
            var query = _userManager.Users
                .Where(p => p.Roles.Any(r => r.RoleId == roleAccount.Id));
            var user = await query.Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            // var user = await db.Users.FindAsync(id);
            //return this.ApiOk(user.ToApiModel<AccountApiModel>());
            if(user == null)
            {
                return new NotFoundObjectResult("Account with id was not found!");
            }
            var viewmodel = _accountMapper.Map(user);
            return new OkObjectResult(viewmodel);
        }
    }
}
