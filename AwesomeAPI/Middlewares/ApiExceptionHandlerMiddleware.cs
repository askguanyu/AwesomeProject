using System;
using System.Net;
using System.Threading.Tasks;
using AwesomeLib.Services.Interfaces;
using AwesomeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AwesomeAPI.Middlewares
{
    public class ApiExceptionHandlerMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger<ApiExceptionHandlerMiddleware> _logger;
        readonly IExceptionResolver _exResolver;
        readonly IConverter _converter;
        readonly IResponseProvider _response;

        public ApiExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ApiExceptionHandlerMiddleware> logger,
            IExceptionResolver exResolver,
            IConverter converter,
            IResponseProvider response)
        {
            _next = next;
            _logger = logger;
            _exResolver = exResolver;
            _converter = converter;
            _response = response;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogError(LogEvents.ApiExceptionHandler, ex, _exResolver.GetInnerMessage(ex));

                await context.Response.WriteAsync(_converter.ConvertToJson(_response
                    .AddException(ex)
                    .Get()));
            }
        }
    }
}