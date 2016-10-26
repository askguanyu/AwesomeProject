using System;
using AwesomeLib;
using Microsoft.Extensions.Configuration;

namespace AwesomeServer.Configuration
{
    public class ConfigBase
    {
        protected static IConfigurationRoot Configuration;

        static ConfigBase()
        {
            var builder = AwesomeMethods.CreateConfigurationBuilder("hosting");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                builder.AddUserSecrets();
            }
            Configuration = builder.Build();
        }
    }
}