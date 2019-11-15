using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AccountCommands
{
    public class GetMyAccountCommand : IGetMyAccountCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, Account> _userMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyAccountCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, Account> userMapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return new NotFoundResult();

            var claims = user.Claims.ToList();
            if (claims.Count < 1)
                return new NotFoundResult();

            var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;
            var account = await _userManager.FindByIdAsync(userId);
            
            var viewModel = _userMapper.Map(account);
            return new OkObjectResult(viewModel);
        }
    }
}
