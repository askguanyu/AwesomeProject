using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AwesomeAPI.Middlewares
{
    public class ExceptionRaiserMiddleware
    {
        readonly RequestDelegate _next;

        public ExceptionRaiserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            RaiseException();
            await _next.Invoke(context);
        }

        void RaiseException()
        {
            throw new Exception("Test Exception");
        }
    }
}