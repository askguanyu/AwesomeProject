using AwesomeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AwesomeAPI.DatabaseContext.Builders
{
    public class ActivityModelBuilder : ModelBuilderBase<Activity>
    {
        public ActivityModelBuilder(ModelBuilder modelBuilder) :
            base(modelBuilder)
        {
        }

        protected override void BuildColumns()
        {
            base.BuildColumns();
            Entity.Property(x => x.Subject)
                .HasMaxLength(50)
                .IsRequired();
            Entity.Property(x => x.Details)
                .HasMaxLength(255);
        }

        protected override void BuildRelations()
        {
            Entity
                .HasOne(activity => activity.ActivityType)
                .WithMany(activityType => activityType.Activities)
                .HasForeignKey(activity => activity.ActivityTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}