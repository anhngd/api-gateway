using Boxed.AspNetCore;
using IdentityService.ViewModels.SignUpViewModels;

namespace IdentityService.Commands.UserCommands
{
    public interface IPostSignUpCommand : IAsyncCommand<SignUp>
    {
    }
}
