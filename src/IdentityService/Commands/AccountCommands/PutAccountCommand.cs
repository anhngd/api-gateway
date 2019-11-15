using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class PutAccountCommand : IPutAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, Account> _accountMapper;
        private readonly IMapper<UpdateAccount, ApplicationUser> _updateAccountMapper;

        public PutAccountCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, Account> accountMapper,
            IMapper<UpdateAccount, ApplicationUser> updateAccountMapper)
        {
            _userManager = userManager;
            _accountMapper = accountMapper;
            _updateAccountMapper = updateAccountMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string id, UpdateAccount model, CancellationToken cancellationToken)
        {
            //check null argument
            //if (model.Address == null || model.BirthDate == null || model.FamilyName == null || model.Gender == null ||
            //    model.GivenName == null || model.Locale == null || model.ZoneInfo == null)
            //    return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");
            //
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || !await _userManager.IsInRoleAsync(user, RoleNames.User))
            {
                return new NotFoundObjectResult("Account does not exist.");
            }
            // update user
            _updateAccountMapper.Map(model, user);
            await _userManager.UpdateAsync(user);

            var viewmodel = _accountMapper.Map(user);
            return new OkObjectResult(viewmodel);
        }
    }
}
