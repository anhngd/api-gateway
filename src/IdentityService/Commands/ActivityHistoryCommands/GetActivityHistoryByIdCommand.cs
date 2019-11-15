using System.Threading;
using System.Threading.Tasks;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public class GetActivityHistoryByIdCommand : IGetActivityHistoryByIdCommand
    {
        private readonly IUserRepository _userRepository;

        public GetActivityHistoryByIdCommand(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _userRepository.GetActivityHistoryById(id, cancellationToken);
            if (item == null)
                return new NotFoundResult();
            return new OkObjectResult(item);
        }
    }
}
