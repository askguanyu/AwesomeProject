using AwesomeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AwesomeAPI.DatabaseContext.Builders
{
    public abstract class ModelBuilderBase<TΜodel>
        where TΜodel : ModelBase
    {
        protected EntityTypeBuilder<TΜodel> Entity;

        public ModelBuilderBase(ModelBuilder modelBuilder)
        {
            Entity = modelBuilder.Entity<TΜodel>();
        }

        public void BuildModel()
        {
            BuildTable();
            BuildColumns();
            BuildRelations();
        }

        protected virtual void BuildTable()
        {
        }

        protected virtual void BuildColumns()
        {
            Entity.Property(x => x.InsertedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("DATETIME()");
            Entity.Property(x => x.UpdatedOn)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("DATETIME()");
        }

        protected virtual void BuildRelations()
        {
        }
    }
}