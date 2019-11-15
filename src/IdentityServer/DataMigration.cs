using System;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public static class DataMigration
    {
        public static void Migrate(IServiceProvider serviceProvider)
        {

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<IdentityDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                try
                {
                    //var persistedGrantContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
                    //persistedGrantContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
