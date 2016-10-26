using AwesomeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AwesomeAPI.DatabaseContext.Builders
{
    public class ActivityTypeModelBuilder : ModelBuilderBase<ActivityType>
    {
        public ActivityTypeModelBuilder(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void BuildTable()
        {
            Entity
                .HasIndex(x => new { x.Description })
                .IsUnique();
        }

        protected override void BuildColumns()
        {
            base.BuildColumns();
            Entity.Property(x => x.UserId)
                .HasMaxLength(255)
                .IsRequired();
            Entity.Property(x => x.Description)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}