using Boxed.AspNetCore;
using IdentityService.ViewModels.AccountViewModels;

namespace IdentityService.Commands.AccountCommands
{
    public interface IPutMyAccountCommand : IAsyncCommand<UpdateAccount>
    {
    }
}
