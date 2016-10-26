using AwesomeAPI.DatabaseContext;
using AwesomeAPI.Models;

namespace AwesomeAPI.Repositories
{
    public class ActivityTypeRepository : RepositoryBase<ActivityType>
    {
        public ActivityTypeRepository(ApiDbContext dbContext) :
            base(dbContext)
        {
        }
    }
}