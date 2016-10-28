using System;

namespace AwesomeLib.Services.Interfaces
{
    public interface IExceptionResolver
    {
        string GetInnerMessage(Exception ex);
    }
}