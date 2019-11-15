using Boxed.AspNetCore;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public interface IGetActivityHistoryByIdCommand : IAsyncCommand<int>
    {
    }
}
