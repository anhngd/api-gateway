using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IDeleteAccountUserCommand : IAsyncCommand<string>
    {
    }
}
