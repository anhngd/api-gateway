using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IPutAccountUserCommand : IAsyncCommand<string, string>
    {
    }
}
