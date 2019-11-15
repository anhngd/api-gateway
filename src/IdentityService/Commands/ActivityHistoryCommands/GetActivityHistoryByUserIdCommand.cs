using System.Threading;
using System.Threading.Tasks;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public class GetActivityHistoryByUserIdCommand : IGetActivityHistoryByUserIdCommand
    {
        private readonly IUserRepository _userRepository;

        public GetActivityHistoryByUserIdCommand(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> ExecuteAsync(string userId, CancellationToken cancellationToken)
        {
            var list = await _userRepository.GetActivityHistoryByUserId(userId, cancellationToken);
            if (list == null)
                return new NotFoundResult();
            return new OkObjectResult(list);
        }
    }
}
