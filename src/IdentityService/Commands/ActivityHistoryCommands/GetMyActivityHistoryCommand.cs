using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public class GetMyActivityHistoryCommand : IGetMyActivityHistoryCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyActivityHistoryCommand(
            IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return new UnauthorizedResult();

            var claims = user.Claims.ToList();
            if (claims.Count < 1)
                return new UnauthorizedResult();

            var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;

            var list = await _userRepository.GetActivityHistoryByUserId(userId, cancellationToken);
            if (list == null)
                return new NotFoundResult();
            return new OkObjectResult(list);
        }
    }
}
