using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IPostAccountStatusCommand : IAsyncCommand<string, string>
    {
    }
}
