using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Administration;
using Ocelot.DependencyInjection;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            const string authenticationProviderKey = "IdentityApiKey";

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });
            services.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, x =>
                {
                    x.Authority = Configuration.GetSection("ApiAuthentication").GetSection("Authority").Value;
                    //  x = Configuration.GetSection("ApiAuthentication").GetSection("ApiName").Value;
                    x.RequireHttpsMetadata = bool.Parse(Configuration.GetSection("ApiAuthentication").GetSection("RequireHttpsMetadata").Value);
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidAudiences = new[] { "identity", "catalog", "hotel", "booking" }
                    };
                    x.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            var i = 0;
                        },
                        OnTokenValidated = async ctx =>
                        {

                        },
                        OnMessageReceived = async ctx => Console.WriteLine("-----------------------------------------")
                    };
                });
            services
                .AddOcelot()
                .AddAdministration("/administration", "secret");
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
            app.UseMvc();
        }
    }
}
