using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Linq;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //var migrate = args.Any(x => x == "--migrate");
            //if (migrate) args = args.Except(new[] { "--migrate" }).ToArray();
            //if (migrate)
            //{
            //    DataMigration.Migrate(host.Services);
            //}

            var seed = args.Any(x => x == "--seed");
            if (seed)
            {
                args.Except(new[] { "--seed" }).ToArray();
                SeedData.EnsureSeedData(host.Services);
            }

            if (seed)
            {
                return;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog((context, configuration) =>
                    {
                        configuration
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.File(@"identityserver4_log.txt")
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                    });
        }
    }
}
