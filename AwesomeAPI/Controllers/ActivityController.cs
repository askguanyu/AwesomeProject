using AwesomeLib.Services.Interfaces;
using AwesomeAPI.Models;
using AwesomeAPI.Repositories.Interfaces;
using AwesomeAPI.Services.Interfaces;
using AwesomeAPI.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AwesomeAPI.Controllers
{
    public class ActivityController : ControllerBase<Activity, ActivityViewModel, ActivityController>
    {
        protected override int LogEventId { get { return LogEvents.ActivityController; } }

        public ActivityController(
            IRepository<Activity> repository,
            ILogger<ActivityController> logger,
            IConverter converter,
            IStringLocalizer<SharedResources> localizer,
            IResponseProvider response) :
            base(repository, logger, converter, localizer, response)
        {
        }
    }
}