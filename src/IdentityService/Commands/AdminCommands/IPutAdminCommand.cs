using Boxed.AspNetCore;
using IdentityService.ViewModels.AdminViewModels;

namespace IdentityService.Commands.AdminCommands
{
    public interface IPutAdminCommand : IAsyncCommand<string, UpdateAdmin>
    {
    }
}
