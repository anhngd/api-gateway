using Boxed.AspNetCore;
using IdentityService.ViewModels.AccountViewModels;

namespace IdentityService.Commands.AccountCommands
{
    public interface IPutAccountLockCommand : IAsyncCommand<string, LockRequestAccount>
    {
    }
}
