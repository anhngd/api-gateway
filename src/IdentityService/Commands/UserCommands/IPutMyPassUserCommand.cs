using Boxed.AspNetCore;
using IdentityService.ViewModels.UserViewModels;

namespace IdentityService.Commands.UserCommands
{
    public interface IPutMyPassUserCommand : IAsyncCommand<UpdatePassword>
    {
    }
}
