using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class PutUserCommand : IPutUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<UpdateUser, ApplicationUser> _updateUserToApplicationUserMapper;
        private readonly IMapper<ApplicationUser, User> _applicationUserToUserMapper;

        public PutUserCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<UpdateUser, ApplicationUser> updateUserToApplicationUserMapper,
            IMapper<ApplicationUser, User> applicationUserToUserMapper)
        {
            _userManager = userManager;
            _updateUserToApplicationUserMapper = updateUserToApplicationUserMapper;
            _applicationUserToUserMapper = applicationUserToUserMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string id, UpdateUser model, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult("User does not exist.");
            }

            _updateUserToApplicationUserMapper.Map(model, user);
            await _userManager.UpdateAsync(user);
            var viewModel = _applicationUserToUserMapper.Map(user);
            return new OkObjectResult(viewModel);
        }
    }
}
