using System.IO;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Library
{
    static class ConfigurationExtension
    {
        public static IConfiguration AppSetting { get; }
        static ConfigurationExtension()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
