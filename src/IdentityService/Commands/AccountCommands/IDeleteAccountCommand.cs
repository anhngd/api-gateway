using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IDeleteAccountCommand : IAsyncCommand<string>
    {
    }
}
