using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ModelBuilderExtensions
{
    public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.Relational().TableName = entityType.DisplayName();
        }
    }
}