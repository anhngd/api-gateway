using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IDeleteUserCommand : IAsyncCommand<string>
    {
    }
}
