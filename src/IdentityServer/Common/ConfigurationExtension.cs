namespace IdentityServer.Common
{
    using Microsoft.Extensions.Configuration;
    using System.IO;

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
