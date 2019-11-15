using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IGetUserCommand : IAsyncCommand<string>
    {
    }
}
