using AwesomeAPI.Middlewares;
using Microsoft.AspNetCore.Builder;

public static class ApiExceptionHandlerExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
}