using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwesomeLib.Services.Interfaces
{
    public interface IModelStateResolver
    {
        IEnumerable<string> GetErrors(ModelStateDictionary modelState);
    }
}