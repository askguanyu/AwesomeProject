using AwesomeAPI.DatabaseContext;
using AwesomeAPI.Models;

namespace AwesomeAPI.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>
    {
        public ActivityRepository(ApiDbContext dbContext) :
            base(dbContext)
        {
        }
    }
}