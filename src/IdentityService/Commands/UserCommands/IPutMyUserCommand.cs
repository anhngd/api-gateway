using Boxed.AspNetCore;
using IdentityService.ViewModels.UserViewModels;

namespace IdentityService.Commands.UserCommands
{
    public interface IPutMyUserCommand : IAsyncCommand<UpdateUser>
    {
    }
}
