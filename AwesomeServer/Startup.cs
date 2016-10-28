using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using AwesomeLib;
using AwesomeServer.Controllers.Attributes;
using AwesomeServer.DatabaseContext;
using AwesomeServer.Services;
using AwesomeServer.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace AwesomeServer
{
    public class Startup
    {
        // We add the cultures as public to use them in the Views
        public static Dictionary<string, string> SupportedCultures = new Dictionary<string, string> {
            { "en-US", "English" },
            { "el-GR", "Ελληνικά" }
        };

        readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            // Load configuration
            _configuration = LoadConfiguration(env);
        }

        IConfigurationRoot LoadConfiguration(IHostingEnvironment env)
        {
            var builder = AwesomeMethods.CreateConfigurationBuilder("appsettings")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // In development environment we add user secrets
                builder.AddUserSecrets();
            };

            return builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use loaded configuration to add options
            ConfigureOptions(services);

            // Add Kernel Https support. This is meant for production environment only. We need to 
            // provide an appsettings.Production.json file with a .pfx file (CertificateFile) and
            // a password (CertificatePassword) in order for this to work.
            ConfigureKestrelHttps(services);

            // Add Identity Db Context
            services.AddDbContext<ServerDbContext>();

            // Add localization and set resources path to Resources/
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Add email service
            services.AddTransient<IEmailSender, EmailSender>();

            // Add Identity
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                // Default values
                // config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // config.Password.RequireDigit = true;
                // config.Password.RequiredLength = 6;
                // config.Password.RequireLowercase = true;
                // config.Password.RequireNonAlphanumeric = true;
                // config.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<ServerDbContext>()
                .AddDefaultTokenProviders(); // This is used for email confirmation and password reset

            // Require SSL for the production environment
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(ProductionRequireHttpsAttribute));
            });

            // Add MVC and enable localization for Views and ViewModel validation messages
            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            // Add IdentityServer
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer()
                .AddOperationalStore(
                    builder => builder.UseSqlite(_configuration["DbConnection"], options => options.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(
                    builder => builder.UseSqlite(_configuration["DbConnection"], options => options.MigrationsAssembly(migrationsAssembly)))
                .AddAspNetIdentity<IdentityUser>();

            // Add seeder for the db initial data
            services.AddTransient<SeedDbData>();

            // Add custom services from our Awesome library (AwesomeLib)
            services.AddAwesome();
        }

        void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ServerOptions>(_configuration);
            services.Configure<ServerOptions>(options =>
            {
                // The below keys are user's secret variables and are loaded in development environment
                if (_configuration["EmailServer"] != null)
                {
                    options.EmailOptions.Server = _configuration["EmailServer"];
                }
                if (_configuration["EmailPort"] != null)
                {
                    options.EmailOptions.Port = Convert.ToInt32(_configuration["EmailPort"]);
                }
                if (_configuration["EmailSSL"] != null)
                {
                    options.EmailOptions.SSL = Convert.ToBoolean(_configuration["EmailSSL"]);
                }
                if (_configuration["EmailAccount"] != null)
                {
                    options.EmailOptions.Account = _configuration["EmailAccount"];
                }
                if (_configuration["EmailPassword"] != null)
                {
                    options.EmailOptions.Password = _configuration["EmailPassword"];
                }
            });
        }

        void ConfigureKestrelHttps(IServiceCollection services)
        {
            if (_configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
            {
                var certificateFile = Path.Combine(Directory.GetCurrentDirectory(), _configuration["CertificateFile"]);
                X509Certificate2 certificate = new X509Certificate2(certificateFile, _configuration["CertificatePassword"]);
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.UseHttps(certificate);
                });
            }
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<ServerOptions> serverOptions,
            SeedDbData seeder)
        {
            // When in development
            if (env.IsDevelopment())
            {
                // Log in Debug Window and in Console using the minimum log level from options
                loggerFactory.AddDebug(serverOptions.Value.LogLevel);
                loggerFactory.AddConsole(serverOptions.Value.LogLevel);

                // Use developer exception pages
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            // When in staging/production
            else
            {
                // Log in files (using the Serilog library) using the minimum log level from options
                AwesomeMethods.CreateSerilogLogger(env, serverOptions.Value.LogLevel);
                loggerFactory.AddSerilog();

                // Use custom exception page
                app.UseExceptionHandler("/Home/Error");
            }

            // Add custom middlware to log exceptions and fall back to the previous exception handlers
            app.UseLogExceptionHandler();

            // Configure the localization. en-US and el-GR cultures are supported with en-US being
            // the default one
            ConfigureLocalization(app);

            // Enable static files so files in wwwroot can be found and served
            app.UseStaticFiles();

            // Apply initial data in the database
            seeder.EnsureSeedInitialDataAsync().Wait(); ;

            // Enable Identity
            app.UseIdentity();

            // Enable IdentityServer
            app.UseIdentityServer();

            // Enable MVC with a default template
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }

        void ConfigureLocalization(IApplicationBuilder app)
        {
            var cultures = new List<CultureInfo>();
            foreach (var culture in SupportedCultures)
            {
                cultures.Add(new CultureInfo(culture.Key));
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });
        }
    }
}