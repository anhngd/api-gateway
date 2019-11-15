using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityService.Library;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class PutMyUserCommand : IPutMyUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, User> _userMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper<UpdateUser, ApplicationUser> _updateUserToApplicationUserMapper;
        private readonly IdentityDbContext _context;

        public PutMyUserCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, User> userMapper,
            IMapper<UpdateUser, ApplicationUser> updateUserToApplicationUserMapper,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _updateUserToApplicationUserMapper = updateUserToApplicationUserMapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(UpdateUser model, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return new NotFoundResult();

            var claims = user.Claims.ToList();
            if (claims.Count < 1)
                return new NotFoundResult();

            var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;
            var account = await _userManager.FindByIdAsync(userId);

            _updateUserToApplicationUserMapper.Map(model, account);

            await _userManager.UpdateAsync(account);

            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, account, "update");

            var viewModel = _userMapper.Map(account);

            return new OkObjectResult(viewModel);
        }
    }
}
