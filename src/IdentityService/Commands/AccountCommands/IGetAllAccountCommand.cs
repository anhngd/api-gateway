using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IGetAllAccountCommand : IAsyncCommand<string, int, int>
    {
    }
}
