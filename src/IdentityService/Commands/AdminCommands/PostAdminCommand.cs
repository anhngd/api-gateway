using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using IdentityServer.Constants;
using IdentityServer.Models;
using IdentityService.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Commands.AdminCommands
{
    public class PostAdminCommand : IPostAdminCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper<ApplicationUser, Admin> _adminMapper;
        private readonly IMapper<CreateAdmin, ApplicationUser> _createAdminMapper;

        public PostAdminCommand(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper<ApplicationUser, Admin> adminMapper,
            IMapper<CreateAdmin, ApplicationUser> createAdminMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _adminMapper = adminMapper;
            _createAdminMapper = createAdminMapper;
        }

        public async Task<IActionResult> ExecuteAsync(CreateAdmin model, CancellationToken cancellationToken)
        {
            // check null argument
            if (model.Address == null || model.BirthDate == null || model.Email == null || model.FamilyName == null ||
                model.Gender == null || model.GivenName == null || model.Locale == null || model.Password == null ||
                model.PhoneNumber == null || model.Picture == null || model.UserName == null)
                return new BadRequestObjectResult("ArgumentNullException: Value cannot be null.");
            // check user name
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new BadRequestObjectResult("ERR_CFL_USNAME: This user name is already taken. Choose a different name.");
            }
            // Check email
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new BadRequestObjectResult("ERR_CFL_EMAIL: This email address is not available. Choose a different address.");
            }
            // create user ---
            var user = _createAdminMapper.Map(model);
            // administrators do not need to verify confirm email
            user.EmailConfirmed = true;
            var role = await _roleManager.FindByNameAsync(RoleNames.Admin);
            user.RoleId = role.Id;
            // Creating new user
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return new BadRequestObjectResult(errors);
            }
            // add user to role Admin ---
            result = _userManager.AddToRoleAsync(user, RoleNames.Admin).Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            // ---

            //var itemUrl = Url.Action(nameof(GetById), ControllerName, new
            //{
            //    id = user.Id
            //});

            var url = "/api/admins/" + user.Id;
            var viewmodel = _adminMapper.Map(user);
            return new CreatedResult(url, viewmodel);
        }
    }
}
