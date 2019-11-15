using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Library
{
    public static class ActivityHistoryExtension
    {
        public static async Task<ActivityHistory> CreateAsync(
            IHttpContextAccessor _httpContextAccessor,
            UserManager<ApplicationUser> _userManager,
            IdentityDbContext _context,
            ApplicationUser user,
            string action)
        {
            // log action login
            var history = new ActivityHistory();
            //loginHistory.Id = 2;
            history.TimeLogin = DateTime.UtcNow.ToString();
            history.UserId = user.Id;
            
            var clientInfo = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString().Split(' ');

            if (clientInfo.Count() < 5)
            {
                history.Os = clientInfo[0];
                history.Browser = clientInfo[0];
            }
            else
            {
                history.Os = clientInfo[1] + clientInfo[2] + clientInfo[3] + clientInfo[4] + clientInfo[5];
                history.Browser = clientInfo[10];
            }

            history.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (history.IpAddress == "::1")
            {
                history.IpAddress = "localhost";
                history.Location = "localhost";
            }
            else
                history.Location = await GetLocationByIp(history.IpAddress);

            history.Email = user.Email;
            history.Action = action;

            await _context.ActivityHistories.AddAsync(history);
            await _context.SaveChangesAsync();

            return history;
        }

        public static async Task<string> GetLocationByIp(string ipv4)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://api.ipstack.com/" + ipv4 + "?access_key=295c7a7283ae0c13e1dd220da9045f90");
            var responses = response.Split(",");
            
            var country = "country: " + responses[5].Split(":")[1];
            var city = "city: " + responses[8].Split(":")[1];
            var location = country + ", " + city;

            return location;
        }
    }
}
