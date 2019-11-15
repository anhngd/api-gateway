
using System;
using Boxed.AspNetCore;
using CorrelationId;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.AccessTokenValidation;
using IdentityService.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;

namespace IdentityService
{
    /// <summary>
    /// The main start-up class for the application.
    /// </summary>
    public class Startup : IStartup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html</param>
        /// <param name="hostingEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default. See http://docs.asp.net/en/latest/fundamentals/environments.html</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Configures the services to add to the ASP.NET Core Injection of Control (IoC) container. This method gets
        /// called by the ASP.NET runtime. See
        /// http://blogs.msdn.com/b/webdev/archive/2014/06/17/dependency-injection-in-asp-net-vnext.aspx
        /// </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            services
                .AddCorrelationIdFluent()
                .AddCustomCaching()
                .AddCustomOptions(_configuration)
                .AddCustomRouting()
                .AddResponseCaching()
                .AddCustomResponseCompression()
                .AddCustomStrictTransportSecurity()
                .AddCustomHealthChecks()
                .AddCustomSwagger()
                .AddHttpContextAccessor()
                // Add useful interface for accessing the ActionContext outside a controller.
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                // Add useful interface for accessing the IUrlHelper outside a controller.
                .AddScoped(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext))
                .AddCustomApiVersioning()
                .AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'VVV") // Version format: 'v'major[.minor][-status]
                //.AddDbContext<DataContext>(options =>
                //        options.UseMySql(_configuration.GetConnectionString("DefaultConnection")))
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApiExplorer()
                .AddDataAnnotations()
                .AddCustomJsonOptions(_hostingEnvironment)
                .AddCustomCors()
                .AddCustomMvcOptions(_hostingEnvironment)
                .Services
                .AddProjectCommands()
                .AddProjectMappers()
                .AddProjectRepositories()
                .AddProjectServices();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // base-address of your identity server
                    options.Authority = _configuration.GetSection("ApiAuthentication").GetSection("Authority").Value;
                    options.ApiName = _configuration.GetSection("ApiAuthentication").GetSection("ApiName").Value;
                    options.RequireHttpsMetadata = bool.Parse(_configuration.GetSection("ApiAuthentication").GetSection("RequireHttpsMetadata").Value);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApplicationPolicies.Root, builder =>
                    builder.RequireScope(ApplicationScopes.Root));

                options.AddPolicy(ApplicationPolicies.Admin, builder =>
                    builder.RequireScope(ApplicationScopes.Admin));

                options.AddPolicy(ApplicationPolicies.OnlineUser, builder =>
                    builder.RequireScope(ApplicationScopes.OnlineUser));
            });

            services.AddDbContext<IdentityDbContext>(builder =>
            {
                builder.UseMySql(_configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                    sqlOptions.MigrationsAssembly(migrationsAssembly));
            });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddIdentityManagers<IdentityDbContext, ApplicationUser, IdentityRole>()
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures the application and HTTP request pipeline. Configure is called after ConfigureServices is
        /// called by the ASP.NET runtime.
        /// </summary>
        public void Configure(IApplicationBuilder application) =>
            application
                // Pass a GUID in a X-Correlation-ID HTTP header to set the HttpContext.TraceIdentifier.
                // UpdateTraceIdentifier must be false due to a bug. See https://github.com/aspnet/AspNetCore/issues/5144
                .UseCorrelationId(new CorrelationIdOptions() { UpdateTraceIdentifier = false })
                .UseForwardedHeaders()
                .UseResponseCaching()
                .UseResponseCompression()
                .UseCors(CorsPolicyName.AllowAny)
                .UseIf(
                    !_hostingEnvironment.IsDevelopment(),
                    x => x.UseHsts())
                .UseIf(
                    _hostingEnvironment.IsDevelopment(),
                    x => x.UseDeveloperErrorPages())
                .UseAuthentication()
                .UseHealthChecks("/status")
                .UseHealthChecks("/status/self", new HealthCheckOptions() { Predicate = _ => false })
                .UseStaticFilesWithCacheControl()
                .UseMvc()
                .UseSwagger()
                .UseCustomSwaggerUi();
    }
}