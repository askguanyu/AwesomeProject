using AwesomeLib;
using AwesomeLib.Services.Interfaces;
using AwesomeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AwesomeAPI.Filters
{
    public class ValidateViewModelAttribute : TypeFilterAttribute
    {
        public ValidateViewModelAttribute() : base(typeof(ValidateViewModelFilter))
        {
        }

        class ValidateViewModelFilter : IActionFilter
        {
            readonly ILogger<ValidateViewModelFilter> _logger;
            readonly IConverter _converter;
            readonly IResponseProvider _response;

            public ValidateViewModelFilter(
                ILogger<ValidateViewModelFilter> logger,
                IConverter converter,
                IResponseProvider response)
            {
                _logger = logger;
                _converter = converter;
                _response = response;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.ActionArguments.ContainsKey("viewModel") &&
                    context.ActionArguments["viewModel"] != null)
                {
                    _logger.LogTrace(LogEvents.ValidateViewModel, AwesomeMethods.ConvertDataForLog(context.ActionArguments["viewModel"], _converter));
                }

                if (!context.ModelState.IsValid)
                {
                    _logger.LogError(LogEvents.ValidateViewModel, "INVALID MODEL");
                    _logger.LogError(LogEvents.ValidateViewModel, AwesomeMethods.ConvertDataForLog(context.ModelState, _converter));
                    context.Result = new BadRequestObjectResult(_response.AddModelState(context.ModelState)
                        .Get());
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}