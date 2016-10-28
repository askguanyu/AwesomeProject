using System;
using AwesomeLib.Services.Interfaces;

namespace AwesomeLib.Services
{
    public class ExceptionResolver : IExceptionResolver
    {
        public string GetInnerMessage(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }
}