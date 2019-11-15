using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityService.Library;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.ActivityHistoryCommands
{
    public class PostActivityHistoryCommand : IPostActivityHistoryCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityDbContext _context;

        public PostActivityHistoryCommand(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IdentityDbContext context)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
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
            var account = await _userManager.FindByIdAsync(userId);
            
            var model = await ActivityHistoryExtension.CreateAsync(_httpContextAccessor, _userManager, _context, account, "login");

            return new OkObjectResult(model);
        }
    }
}
