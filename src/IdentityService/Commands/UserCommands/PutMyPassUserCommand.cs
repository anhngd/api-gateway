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
    public class PutMyPassUserCommand : IPutMyPassUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, User> _userMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _context;

        public PutMyPassUserCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, User> userMapper,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(UpdatePassword model, CancellationToken cancellationToken)
        {
            if (model.CurrentPassword == null || model.NewPassword == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");

            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return new NotFoundResult();

            var claims = user.Claims.ToList();
            if(claims.Count < 1)
                return new NotFoundResult();

            var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;
            var account = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
                return new BadRequestObjectResult("Current password is invalid.");

            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, account, "update");

            return new OkObjectResult("The password of account was updated!");
        }
    }
}
