using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Models;
using IdentityService.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Commands.UserCommands
{
    public class GetAllUserCommand : IGetAllUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper<ApplicationUser, User> _userMapper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllUserCommand(
            UserManager<ApplicationUser> userManager,
            IMapper<ApplicationUser, User> userMapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> ExecuteAsync(string sort, int limit, int page, CancellationToken cancellationToken)
        {
            // var user = _httpContextAccessor.HttpContext.User;
            if (sort != "email" && sort != "birth_date" && sort != "family_name" && sort != "name" && sort != "gender" && sort != "user_name")
            {
                sort = "_id";
            }
            if (!(limit < 100 && limit > 1)) { limit = 20; }
            if (!(page > 1)) { page = 1; }

            var query = _userManager.Users;

            switch (sort)
            {
                case "_id":
                    query = query.OrderBy(q => q.Id);
                    break;
                case "email":
                    query = query.OrderBy(q => q.NormalizedEmail);
                    break;
                case "birth_date":
                    query = query.OrderBy(q => q.BirthDate);
                    break;
                case "family_name":
                    query = query.OrderBy(q => q.FamilyName);
                    break;
                case "name":
                    query = query.OrderBy(q => q.GivenName);
                    break;
                case "gender":
                    query = query.OrderBy(q => q.Gender);
                    break;
                case "user_name":
                    query = query.OrderBy(q => q.UserName);
                    break;
            }

            var skip = (page - 1) * limit;

            var tCount = query.CountAsync(cancellationToken: cancellationToken);
            var tData = query
                    .Include(u => u.Role)
                    .Select(u => _userMapper.Map(u))
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync(cancellationToken: cancellationToken);

            await Task.WhenAll(tCount, tData);

            var count = tCount.Result;
            var data = tData.Result;

            return new OkObjectResult(new
            {
                count,
                data,
            });
        }
    }
}
