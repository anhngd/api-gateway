using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IPostConfirmEmailCommand : IAsyncCommand<string, string>
    {
    }
}
