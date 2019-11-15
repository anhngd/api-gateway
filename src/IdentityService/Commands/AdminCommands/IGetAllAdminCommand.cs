using Boxed.AspNetCore;

namespace IdentityService.Commands.AdminCommands
{
    public interface IGetAllAdminCommand : IAsyncCommand<string, int, int>
    {
    }
}
