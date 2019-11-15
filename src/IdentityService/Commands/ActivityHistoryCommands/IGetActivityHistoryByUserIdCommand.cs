using Boxed.AspNetCore;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public interface IGetActivityHistoryByUserIdCommand : IAsyncCommand<string>
    {
    }
}
