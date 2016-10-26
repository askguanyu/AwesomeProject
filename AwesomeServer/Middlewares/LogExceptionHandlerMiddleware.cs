using System;
using System.Threading.Tasks;
using AwesomeLib.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AwesomeServer.Middlewares
{
    public class LogExceptionHandlerMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger<LogExceptionHandlerMiddleware> _logger;
        readonly IExceptionResolver _exResolver;

        public LogExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<LogExceptionHandlerMiddleware> logger,
            IExceptionResolver exResolver)
        {
            _next = next;
            _logger = logger;
            _exResolver = exResolver;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                // Log exception and throw again so it will be handled as supposed from the request's pipeline
                _logger.LogError(LogEvents.LogExceptionHandler, ex, _exResolver.GetInnerMessage(ex));
                throw;
            }
        }
    }
}