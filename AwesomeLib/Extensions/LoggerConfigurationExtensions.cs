using Microsoft.Extensions.Logging;
using Serilog;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration ApplyMinimumLevel(this LoggerConfiguration configuration, LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                return configuration.MinimumLevel.Verbose();
            case LogLevel.Debug:
                return configuration.MinimumLevel.Debug();
            case LogLevel.Information:
                return configuration.MinimumLevel.Information();
            case LogLevel.Warning:
                return configuration.MinimumLevel.Warning();
            case LogLevel.Error:
                return configuration.MinimumLevel.Error();
            case LogLevel.Critical:
                return configuration.MinimumLevel.Fatal();
        }
        return configuration;
    }
}