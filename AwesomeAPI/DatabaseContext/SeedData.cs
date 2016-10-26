using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeAPI.Models;

namespace AwesomeAPI.DatabaseContext
{
    public class SeedDbData
    {
        readonly ApiDbContext _dbContext;

        public SeedDbData(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task EnsureSeedInitialDataAsync()
        {
            if (!_dbContext.ActivityTypes.Any())
            {
                var activityTypes = new List<ActivityType>
                {
                    new ActivityType {
                        UserId = "E6C344FA-2310-4A25-81E4-694B7A122682",
                        Description = "Appointment"
                    },
                    new ActivityType {
                        UserId = "E6C344FA-2310-4A25-81E4-694B7A122682",
                        Description = "Task"
                    }
                };

                _dbContext.ActivityTypes.AddRange(activityTypes);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}