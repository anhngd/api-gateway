using Boxed.AspNetCore;

namespace IdentityService.Commands.UserCommands
{
    public interface IPutUnLockUserCommand : IAsyncCommand<string>
    {
    }
}
