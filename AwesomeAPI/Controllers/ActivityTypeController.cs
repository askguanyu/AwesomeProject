using AwesomeLib.Services.Interfaces;
using AwesomeAPI.Models;
using AwesomeAPI.Repositories.Interfaces;
using AwesomeAPI.Services.Interfaces;
using AwesomeAPI.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AwesomeAPI.Controllers
{
    public class ActivityTypeController : ControllerBase<ActivityType, ActivityTypeViewModel, ActivityTypeController>
    {
        protected override int LogEventId { get { return LogEvents.ActivityTypeController; } }

        public ActivityTypeController(
            IRepository<ActivityType> repository,
            ILogger<ActivityTypeController> logger,
            IConverter converter,
            IStringLocalizer<SharedResources> localizer,
            IResponseProvider response) :
            base(repository, logger, converter, localizer, response)
        {
        }
    }
}