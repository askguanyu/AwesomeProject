using System.Collections.Generic;
using System.Linq;
using AwesomeLib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwesomeLib.Services
{
    public class ModelStateResolver : IModelStateResolver
    {
        readonly IExceptionResolver _exResolver;

        public ModelStateResolver(IExceptionResolver exResolver)
        {
            _exResolver = exResolver;
        }

        public IEnumerable<string> GetErrors(ModelStateDictionary modelState)
        {
            var errors = new List<string>();

            var modelErrors = modelState.Values
                .SelectMany(v => v.Errors);

            errors.AddRange(modelErrors
                .Where(e => !string.IsNullOrEmpty(e.ErrorMessage))
                .Select(e => e.ErrorMessage));

            foreach (var modelError in modelErrors.Where(e => e.Exception != null))
            {
                errors.Add(_exResolver.GetInnerMessage(modelError.Exception));
            }

            return errors;
        }
    }
}