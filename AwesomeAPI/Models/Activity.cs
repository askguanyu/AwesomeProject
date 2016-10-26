using System;

namespace AwesomeAPI.Models
{
    public class Activity : ModelBase
    {
        public int ActivityTypeId { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}