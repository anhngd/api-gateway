using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.Configurations;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Commands.RoleCommands
{
    public class GetRoleCommand : IGetRoleCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AccountConfiguration _accountConfig;
        //private readonly IEmailSender _emailSender;
        private readonly ApiAuthenticationConfiguration _authConfig;
        private readonly IMapper<ApplicationUser, Account> _accountMapper;

        public GetRoleCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AccountConfiguration> accountConfigOptions,
            //IEmailSender emailSender,
            IOptions<ApiAuthenticationConfiguration> authConfigOptions,
            IMapper<ApplicationUser, Account> accountMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accountConfig = accountConfigOptions.Value ?? new AccountConfiguration();
            //_emailSender = emailSender;
            _authConfig = authConfigOptions.Value ?? new ApiAuthenticationConfiguration();
            _accountMapper = accountMapper;
        }

        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null)
            { 
             return new NotFoundObjectResult("No roles found");
            }
            return new ObjectResult(roles);

        }
    }
}
