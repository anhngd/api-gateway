using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityService.Library;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class PutMyAccountCommand : IPutMyAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, Account> _accountMapper;
        private readonly IMapper<UpdateAccount, ApplicationUser> _updateAccountMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _context;

        public PutMyAccountCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, Account> accountMapper,
            IMapper<UpdateAccount, ApplicationUser> updateAccountMapper,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _accountMapper = accountMapper;
            _updateAccountMapper = updateAccountMapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(UpdateAccount model, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return new NotFoundResult();

            var claims = user.Claims.ToList();
            if (claims.Count < 1)
                return new NotFoundResult();

            var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;
            var account = await _userManager.FindByIdAsync(userId);

            // update account
            _updateAccountMapper.Map(model, account);
            await _userManager.UpdateAsync(account);

            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, account, "update");

            var viewmodel = _accountMapper.Map(account);
            return new OkObjectResult(viewmodel);
        }
    }
}
