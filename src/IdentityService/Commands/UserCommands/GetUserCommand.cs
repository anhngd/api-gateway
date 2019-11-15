using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.UserCommands
{
    public class GetUserCommand : IGetUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, User> _applicationUserToUserMapper;

        public GetUserCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, User> applicationUserToUserMapper)
        {
            _userManager = userManager;
            _applicationUserToUserMapper = applicationUserToUserMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return new NotFoundObjectResult("User with id was not found!");
            var viewModel = _applicationUserToUserMapper.Map(user);
            return new OkObjectResult(viewModel);
        }
    }
}
