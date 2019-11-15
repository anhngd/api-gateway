using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IPostForgotPasswordCommand : IAsyncCommand<string>
    {
    }
}
