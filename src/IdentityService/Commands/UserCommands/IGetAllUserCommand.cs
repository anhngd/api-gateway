using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IGetAllUserCommand : IAsyncCommand<string, int, int>
    {
    }
}
