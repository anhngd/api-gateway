using Boxed.AspNetCore;
using IdentityService.ViewModels;

namespace IdentityService.Commands.UserCommands
{
    public interface IPostResetPasswordCommand : IAsyncCommand<ResetPassword>
    {
    }
}
