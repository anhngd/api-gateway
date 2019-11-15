using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        private const string DefaultTokenProviderName = "Default";
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            
            // connect to database and run migration
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseMySql(connectionString,
                    sqlOptions =>
                        sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = DefaultTokenProviderName;
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(15);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DefaultDataProtectorTokenProvider<ApplicationUser>>(DefaultTokenProviderName);


            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_0)
                .AddDataAnnotationsLocalization();


            services.Configure<RequestLocalizationOptions>(options =>
            {
             
                var langLocale = Configuration.GetSection("Languages");
                var listCultureInfo = new List<CultureInfo>();
                foreach (var section in langLocale.GetChildren())
                { 
                        listCultureInfo.Add(new CultureInfo(section.GetValue<string>("value")));
                }
                var supportedCultures = listCultureInfo;

                //  new CultureInfo("vi")
                options.DefaultRequestCulture = new RequestCulture(listCultureInfo[0]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
           
            ////
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
            
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "231188560014-8a14s5hgb12q1qcq9ff23vj1l790cvti.apps.googleusercontent.com";
                    options.ClientSecret = "gfqT9-720q8Tl1WeQUSy-hoZ";
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var langLocale = Configuration.GetSection("Languages");
            var listCultureInfo = new List<CultureInfo>();
            foreach (var section in langLocale.GetChildren())
            {
                listCultureInfo.Add(new CultureInfo(section.GetValue<string>("value")));
            }

            var supportedCultures = listCultureInfo;

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(listCultureInfo[0]),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        
        }
        public class MySettings
        {
            public List<string> Locale { get; set; }
        }
    }
}