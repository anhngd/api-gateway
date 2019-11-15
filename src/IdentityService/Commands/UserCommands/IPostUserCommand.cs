using Boxed.AspNetCore;
using IdentityService.ViewModels.UserViewModels;

namespace IdentityService.Commands.UserCommands
{
    public interface IPostUserCommand : IAsyncCommand<CreateUser>
    {
    }
}
