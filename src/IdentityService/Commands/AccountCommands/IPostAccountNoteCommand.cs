using Boxed.AspNetCore;

namespace IdentityService.Commands.AccountCommands
{
    public interface IPostAccountNoteCommand : IAsyncCommand<string, string>
    {
    }
}
