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
    public class PutPassUserCommand : IPutPassUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, User> _applicationUserToUserMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _context;

        public PutPassUserCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, User> applicationUserToUserMapper,
            IHttpContextAccessor httpContextAccessor,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _applicationUserToUserMapper = applicationUserToUserMapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> ExecuteAsync(string id, UpdatePassword model, CancellationToken cancellationToken)
        {
            if (model.CurrentPassword == null || model.NewPassword == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult($"User with ID '{id}' does not exist.");
            }

            await _userManager.ChangePasswordAsync(
                user, model.CurrentPassword, model.NewPassword);

            await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, user, "update");

            return new OkObjectResult("The password of account was updated!");
        }
    }
}
