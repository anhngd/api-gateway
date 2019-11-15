using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Common
{
    public static class ActivityHistoryExtension
    {
        public static async Task CreateAsync(
            IHttpContextAccessor httpContextAccessor, 
            UserManager<ApplicationUser> userManager, 
            IdentityDbContext context,
            ApplicationUser user,
            string action)
        {
            // log action login
            var history = new ActivityHistory();
            //loginHistory.Id = 2;
            history.TimeLogin = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            history.UserId = user.Id;

            var clientInfo = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString().Split(' ');

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

            history.IpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (history.IpAddress == "::1")
            {
                history.IpAddress = "localhost";
                history.Location = "localhost";
            }
            else
                history.Location = await GetLocationByIp(history.IpAddress);

            history.Email = user.Email;
            history.Action = action;

            await context.ActivityHistories.AddAsync(history);
            await context.SaveChangesAsync();
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
