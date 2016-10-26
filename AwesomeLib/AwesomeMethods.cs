using System;
using System.IO;
using AwesomeLib.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AwesomeLib
{
    public class AwesomeMethods
    {
        public static string ConvertDataForLog(object data, IConverter converter)
        {
            // When logging we don't need a camelCase conversion for the data properties
            // BUT we need the json to be formatted in order to be readable
            return converter.ConvertToJson(
                data: data,
                camelCase: false,
                indented: true);
        }

        public static void CreateSerilogLogger(IHostingEnvironment env, LogLevel logLevel)
        {
            // MinimumLevel property of LoggerConfiguration is readonly and is applied by functions so
            // we use a custom extension (ApplyMinimumLevel) to apply minimum log level from settings
            Log.Logger = new LoggerConfiguration()
                .ApplyMinimumLevel(logLevel)
                .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "logs/log-{Date}.txt"))
                .CreateLogger();
        }

        public static IConfigurationBuilder CreateConfigurationBuilder(string fileName)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{fileName}.json", optional: true)
                .AddJsonFile($"{fileName}.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
        }
    }
}