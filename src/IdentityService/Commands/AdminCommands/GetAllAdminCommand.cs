using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Commands.AdminCommands
{
    public class GetAllAdminCommand : IGetAllAdminCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper<ApplicationUser, Admin> _adminMapper;

        public GetAllAdminCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper<ApplicationUser, Admin> adminMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _adminMapper = adminMapper;
        }

        public async Task<IActionResult> ExecuteAsync(string sort, int limit, int page, CancellationToken cancellationToken)
        {
            if (sort != "email" && sort != "birth_date" && sort != "family_name" && sort != "name" && sort != "gender" && sort != "user_name")
            {
                sort = "_id";
            }
            if (!(limit < 100 && limit > 1)) { limit = 20; }
            if (!(page > 1)) { page = 1; }

            var role = await _roleManager.FindByNameAsync(RoleNames.Admin);
            //  db.Roles
            //     .Where(x => x.Name == RoleNames.Admin)
            //     .FirstOrDefaultAsync();

            var query = _userManager.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id));

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

            var tCount = query.CountAsync();
            var tData = query
                    .Include(u => u.Role)
                    .Select(u => _adminMapper.Map(u))
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync();

            await Task.WhenAll(tCount, tData);

            var count = tCount.Result;
            var data = tData.Result;

            return new OkObjectResult(new
            {
                count = count,
                data = data
            });
        }
    }
}
