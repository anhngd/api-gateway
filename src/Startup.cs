using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //  services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddRazorPages();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });


            //  const string identityUrl = "https://account.tripbricks.com";
            //  const string authenticationProviderKey = "IdentityApiKey";

            //services.AddAuthentication()
            //    .AddJwtBearer(authenticationProviderKey, x =>
            //    {
            //        x.Authority = identityUrl;
            //        x.RequireHttpsMetadata = false;
            //        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            //        {
            //            ValidAudiences = new[] { "identity", "catalog", "hotel", "booking" }
            //        };
            //        x.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
            //        {
            //            OnAuthenticationFailed = async ctx =>
            //            {
            //                var i = 0;
            //            },
            //            OnTokenValidated = async ctx =>
            //            {

            //            },
            //            OnMessageReceived = async ctx => Console.WriteLine("-----------------------------------------")
            //        };
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //  app.UseMvc();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
