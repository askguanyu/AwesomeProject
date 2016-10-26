using AwesomeServer.Middlewares;
using Microsoft.AspNetCore.Builder;

public static class LogExceptionHandlerExtensions
{
    public static IApplicationBuilder UseLogExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogExceptionHandlerMiddleware>();
    }
}