using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.Configurations;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Commands.AccountCommands
{
    public class GetAllAccountCommand : IGetAllAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AccountConfiguration _accountConfig;
        //private readonly IEmailSender _emailSender;
        private readonly ApiAuthenticationConfiguration _authConfig;
        private readonly IMapper<ApplicationUser, Account> _accountMapper;

        public GetAllAccountCommand(
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

        public async Task<IActionResult> ExecuteAsync(string sort, int limit, int page, CancellationToken cancellationToken)
        {
            if (sort != "email" && sort != "birth_date" && sort != "family_name" && sort != "name" && sort != "gender" && sort != "user_name")
            {
                sort = "_id";
            }
            if (!(limit < 100 && limit > 1)) { limit = 20; }
            if (!(page > 1)) { page = 1; }

            var roleAccount = await _roleManager.FindByNameAsync(RoleNames.User);
            var query = _userManager.Users
                .Where(p => p.Roles.Any(r => r.RoleId == roleAccount.Id));

            switch (sort)
            {
                case "_id":
                    query = query.OrderBy(q => q.Id);
                    break;
                case "email":
                    query = query.OrderBy(q => q.NormalizedEmail);
                    break;
                case "birth_date":
                    query = query.OrderBy(q => q.BirthDate);
                    break;
                case "family_name":
                    query = query.OrderBy(q => q.FamilyName);
                    break;
                case "name":
                    query = query.OrderBy(q => q.GivenName);
                    break;
                case "gender":
                    query = query.OrderBy(q => q.Gender);
                    break;
                case "user_name":
                    query = query.OrderBy(q => q.UserName);
                    break;
            }

            var skip = (page - 1) * limit;

            var tCount = query.CountAsync();
            var tData = query
                    .Include(u => u.Role)
                    .Select(u => _accountMapper.Map(u))
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync();

            await Task.WhenAll(tCount, tData);

            var count = tCount.Result;
            var data = tData.Result;

            return new OkObjectResult(new
            {
                count = count,
                data = data
            });
        }
    }
}
