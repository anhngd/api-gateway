using Boxed.AspNetCore;

namespace IdentityService.Commands.AdminCommands
{
    public interface IDeleteAdminUserCommand : IAsyncCommand<string>
    {
    }
}
