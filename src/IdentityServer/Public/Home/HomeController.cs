// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Public.Home
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private readonly IdentityDbContext _context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IIdentityServerInteractionService interaction, 
            IHostingEnvironment environment, 
            ILogger<HomeController> logger,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity.IsAuthenticated == false)
                return Redirect("~/account/login");
            
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return Redirect("~/account/login");

            var claims = user.Claims.ToList();
            if (claims.Count < 1)
                return Redirect("~/account/login");

            var userId = claims.FirstOrDefault(claimRecord => claimRecord.Type == "sub").Value;
            var account = await _userManager.FindByIdAsync(userId);

            var profile = new ProfileViewModel();

            profile.FamilyName = account.FamilyName;
            profile.GivenName = account.GivenName;
            profile.Gender = account.Gender;
            profile.Picture = account.Picture;

            var birthDate = Convert.ToDateTime(account.BirthDate);
            profile.BirthDate = birthDate.ToShortDateString().ToString(CultureInfo.CurrentCulture);

            profile.Address = account.Address;
            profile.PhoneNumber = account.PhoneNumber;
            profile.Email = account.Email;

            var loginHistories = _context.ActivityHistories.Where(x=>x.UserId == userId).OrderByDescending(x=>x.TimeLogin).ToList();
            if (loginHistories.Count() > 1)
            {
                //loginHistories.Remove(loginHistories.First());
                //DateTime lastLogin = Convert.ToDateTime(loginHistories.First().TimeLogin).ToLocalTime();
                //profile.LastLogin = lastLogin.ToString();
            }
            else
            {
                profile.LastLogin = "This is first login!";
            }
            return View(profile);
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message == null) return View("Error", vm);
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }

            return View("Error", vm);
        }
    }
}