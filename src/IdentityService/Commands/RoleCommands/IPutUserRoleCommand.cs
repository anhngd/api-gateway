using Boxed.AspNetCore;
using IdentityService.ViewModels.UserViewModels;

namespace IdentityService.Commands.RoleCommands
{
    public interface IPutUserRoleCommand : IAsyncCommand<UpdateUserRole>
    {
    }
}
