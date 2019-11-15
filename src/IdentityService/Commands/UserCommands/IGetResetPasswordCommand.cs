using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IGetResetPasswordCommand : IAsyncCommand<string, string>
    {
    }
}
