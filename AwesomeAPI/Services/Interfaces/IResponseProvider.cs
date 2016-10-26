using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwesomeAPI.Services.Interfaces
{
    public interface IResponseProvider
    {
        IResponseProvider AddInfo(string info);
        IResponseProvider AddWarning(string warning);
        IResponseProvider AddError(string error);
        IResponseProvider AddError(IEnumerable<string> errors);
        IResponseProvider AddData(object data);
        IResponseProvider AddModelState(ModelStateDictionary modelState);
        IResponseProvider AddException(Exception ex);
        object Get();
    }
}