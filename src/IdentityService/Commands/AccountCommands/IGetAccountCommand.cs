using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IGetAccountCommand : IAsyncCommand<string>
    {
    }
}
