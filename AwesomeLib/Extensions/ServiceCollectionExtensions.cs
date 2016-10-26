using AwesomeLib.Services;
using AwesomeLib.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class AwesomeServiceCollectionExtensions
{
    // Extension to register all the library's services
    public static void AddAwesome(this IServiceCollection services)
    {
        services.AddTransient<IConverter, Converter>();
        services.AddTransient<IExceptionResolver, ExceptionResolver>();
        services.AddTransient<IModelStateResolver, ModelStateResolver>();
    }
}