using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AwesomeLib;
using AwesomeAPI.Controllers.Attributes;
using AwesomeAPI.DatabaseContext;
using AwesomeAPI.Models;
using AwesomeAPI.Repositories;
using AwesomeAPI.Repositories.Interfaces;
using AwesomeAPI.Services;
using AwesomeAPI.Services.Interfaces;
using AwesomeAPI.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace AwesomeAPI
{
    public class Startup
    {
        readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            // Load configuration
            _configuration = LoadConfiguration(env);
        }

        IConfigurationRoot LoadConfiguration(IHostingEnvironment env)
        {
            return AwesomeMethods.CreateConfigurationBuilder("appsettings")
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use loaded configuration to add options
            services.AddOptions();
            services.Configure<ApiOptions>(_configuration);

            // Add Kernel Https support. This is meant for production environment only. We need to 
            // provide an appsettings.Production.json file with a .pfx file (CertificateFile) and
            // a password (CertificatePassword) in order for this to work.
            ConfigureKestrelHttps(services);

            // Add Db Context
            services.AddDbContext<ApiDbContext>();

            // Add seeder for the db initial data
            services.AddTransient<SeedDbData>();

            // Add repository for every model 
            AddRepositoryServices(services);

            // Add localization and set resources path to Resources/
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Require SSL for the production environment
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(ProductionRequireHttpsAttribute));
            });

            // Add MVC and enable localization for ViewModel validation messages
            services.AddMvc()
                .AddDataAnnotationsLocalization();

            // Add custom services from our Awesome library (AwesomeLib)
            services.AddAwesome();

            // Add custom service to build the response for every request. We use AddScoped()
            // to get the same service instance inside the whole request's pipeline   
            services.AddScoped<IResponseProvider, ResponseProvider>();
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

        void AddRepositoryServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<Activity>, ActivityRepository>();
            services.AddScoped<IRepository<ActivityType>, ActivityTypeRepository>();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<ApiOptions> apiOptions,
            SeedDbData seeder)
        {
            // When in development
            if (env.IsDevelopment())
            {
                // Log in Debug Window and in Console using the minimum log level from options
                loggerFactory.AddDebug(apiOptions.Value.LogLevel);
                loggerFactory.AddConsole(apiOptions.Value.LogLevel);
            }
            // When in staging/production
            else
            {
                // Log in files (using the Serilog library) using the minimum log level from options
                AwesomeMethods.CreateSerilogLogger(env, apiOptions.Value.LogLevel);
                loggerFactory.AddSerilog();
            }

            // Custom Middleware for exception handling. This is added first in the pipeline to catch
            // unhandled exceptions from the whole request
            app.UseApiExceptionHandler();

            // Uncomment the line below to raise exception for debugging purposes to test the above handler
            // app.UseExceptionRaiser();

            // Configure the localization. en-US and el-GR cultures are supported with en-US being 
            // the default one
            ConfigureLocalization(app);

            // Configure the field mapping between Models and ViewModels
            ConfigureMapping();

            // Enable MVC. No routes specified here. We use attribute routing in controllers
            app.UseMvc();

            // Apply initial data in the database
            seeder.EnsureSeedInitialDataAsync().Wait();
        }

        void ConfigureLocalization(IApplicationBuilder app)
        {
            var cultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("el-GR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });
        }

        void ConfigureMapping()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Activity, ActivityViewModel>().ReverseMap();
                config.CreateMap<ActivityType, ActivityTypeViewModel>().ReverseMap();
            });
        }
    }
}