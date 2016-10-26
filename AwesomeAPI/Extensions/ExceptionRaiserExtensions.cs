using AwesomeAPI.Middlewares;
using Microsoft.AspNetCore.Builder;

public static class ExceptionRaiserExtensions
{
    public static IApplicationBuilder UseExceptionRaiser(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionRaiserMiddleware>();
    }
}