using System.Threading;
using System.Threading.Tasks;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public class GetAllActivityHistoryCommand : IGetAllActivityHistoryCommand
    {
        private readonly IUserRepository _userRepository;

        public GetAllActivityHistoryCommand(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var list = await _userRepository.GetAllActivityHistory(cancellationToken);
            if (list == null)
                return new NotFoundResult();
            return new OkObjectResult(list);
        }
    }
}
