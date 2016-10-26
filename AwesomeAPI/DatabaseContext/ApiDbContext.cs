using AwesomeAPI.DatabaseContext.Builders;
using AwesomeAPI.Models;
using AwesomeAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AwesomeAPI.DatabaseContext
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }

        readonly IOptions<ApiOptions> _apiOptions;

        public ApiDbContext(
            DbContextOptions<ApiDbContext> options,
            IOptions<ApiOptions> apiOptions)
            : base(options)
        {
            _apiOptions = apiOptions;
            Database.MigrateAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_apiOptions.Value.DbConnection);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RemovePluralizingTableNameConvention();
            new ActivityModelBuilder(modelBuilder).BuildModel();
            new ActivityTypeModelBuilder(modelBuilder).BuildModel();
        }
    }
}