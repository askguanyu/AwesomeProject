using System.Collections.Generic;

namespace AwesomeAPI.Models
{
    public class ActivityType : ModelBase
    {
        public string UserId { get; set; }
        public string Description { get; set; }
        public List<Activity> Activities { get; set; }
    }
}