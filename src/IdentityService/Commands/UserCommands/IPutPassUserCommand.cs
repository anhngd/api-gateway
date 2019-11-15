using Boxed.AspNetCore;
using IdentityService.ViewModels.UserViewModels;

namespace IdentityService.Commands.UserCommands
{
    public interface IPutPassUserCommand : IAsyncCommand<string, UpdatePassword>
    {
    }
}
